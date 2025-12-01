using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Enemy enemy;
    private Transform player;

    private Vector3 patrolCenter;
    private Vector3 patrolStart;
    private Vector3 patrolTarget;

    private float attackCooldown = 2.5f; // 초기 공격 쿨타임 설정

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어 오브젝트를 태그로 찾기 (플레이어 인스펙터에서 태그 설정 필요)

        patrolCenter = transform.position; // 현재 위치를 패트롤 가운데로 설정

        if (enemy.data.patrolType == PatrolType.Vertical) // 상하 패트롤일 경우
        {
            patrolStart = patrolCenter + new Vector3(0, -(enemy.data.patrolDistance/2), 0); // 시작 지점 설정 (가운데에서 거리의 절반만큼 아래)
            patrolTarget = patrolCenter + new Vector3(0, (enemy.data.patrolDistance/2), 0); // 목표 지점 설정 (가운데에서 거리의 절반만큼 위)
        }
        else if (enemy.data.patrolType == PatrolType.Horizontal) // 좌우 패트롤일 경우
        {
            patrolStart = patrolCenter + new Vector3(-(enemy.data.patrolDistance/2), 0, 0); // 시작 지점 설정 (가운데에서 거리의 절반만큼 왼쪽)
            patrolTarget = patrolCenter + new Vector3((enemy.data.patrolDistance/2), 0, 0); // 목표 지점 설정 (가운데에서 거리의 절반만큼 오른쪽)
        }
    }

    private void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position); // 플레이어와의 거리 계산

        // 플레이어 인식 범위 안에 들어오는 체크
        if (dist <= enemy.data.detectRange)
        {
            // 공격 범위안에 들어오는지 체크
            if (dist <= enemy.data.attackRange)
            {
                TryAttack(); // 들어오면 공격 시도
            }
            else
            {
                MoveToPlayer(); // 공격 범위 밖이면 플레이어에게 이동
            }
        }
        else
        {
            Patrol();
            return; // 인식 범위 밖이면 패트롤
        }

        attackCooldown -= Time.deltaTime; // 공격 쿨타임 감소
    }

    private void TryAttack() // 공격 시도 함수
    {
        if (attackCooldown > 0f) return; // 쿨타임이 남아있으면 공격 불가

        float dmg = enemy.data.attackDamage; // 적의 데미지 가져오기

        Debug.Log($"적의 공격! Damage: {dmg}"); // 디버그 로그 출력 (나중에 플레이어에게 데미지 주는 코드로 변경 필요)

        // TODO: 플레이어에게 데미지 주기

        attackCooldown = enemy.data.attackRate; // 공격 쿨타임 초기화
    }

    private void MoveToPlayer() // 플레이어에게 이동 함수
    {
        Vector3 dir = (player.position - transform.position).normalized; // 플레이어 방향 계산

        transform.position += dir * enemy.data.moveSpeed * Time.deltaTime; // 플레이어 방향으로 이동

        RotateTowardsDirection(dir); // 이동 방향으로 회전
    }

    private void RotateTowardsDirection(Vector3 dir) // 방향으로 회전 함수
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; // 방향 벡터로 각도 계산

        transform.rotation = Quaternion.Euler(0, 0, angle-90); // 계산된 각도로 회전 (스프라이트 기본 방향이 위쪽이므로 -90도 보정)
    }

    private void Patrol() // 패트롤 함수
    {
        if (enemy.data.patrolType == PatrolType.None) // 패트롤 타입이 없으면 패트롤하지 않음
            return;

        transform.position = Vector3.MoveTowards(transform.position, patrolTarget, enemy.data.patrolSpeed * Time.deltaTime); // 목표 지점으로 이동

        Vector3 dir = (patrolTarget - transform.position).normalized; // 이동 방향 계산
        RotateTowardsDirection(dir); // 이동 방향으로 회전

        // 목표 지점 도달 시 반대 방향으로 변경
        if (Vector3.Distance(transform.position, patrolTarget) < 0.1f)
        {
            if (enemy.data.patrolType == PatrolType.Vertical) // 상하 패트롤
            {
                patrolTarget = patrolTarget.y > patrolStart.y ? patrolStart : patrolStart + new Vector3(0, enemy.data.patrolDistance, 0); // 반대 지점으로 변경
            }

            else if (enemy.data.patrolType == PatrolType.Horizontal) // 좌우 패트롤
            {
                patrolTarget = patrolTarget.x > patrolStart.x ? patrolStart : patrolStart + new Vector3(enemy.data.patrolDistance, 0, 0); // 반대 지점으로 변경
            }
        }
    }
}
