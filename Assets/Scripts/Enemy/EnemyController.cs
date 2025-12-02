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
            patrolStart = patrolCenter + new Vector3(0, -(enemy.data.patrolDistance / 2), 0); // 시작 지점 설정 (가운데에서 거리의 절반만큼 아래)
            patrolTarget = patrolCenter + new Vector3(0, (enemy.data.patrolDistance / 2), 0); // 목표 지점 설정 (가운데에서 거리의 절반만큼 위)
        }
        else if (enemy.data.patrolType == PatrolType.Horizontal) // 좌우 패트롤일 경우
        {
            patrolStart = patrolCenter + new Vector3(-(enemy.data.patrolDistance / 2), 0, 0); // 시작 지점 설정 (가운데에서 거리의 절반만큼 왼쪽)
            patrolTarget = patrolCenter + new Vector3((enemy.data.patrolDistance / 2), 0, 0); // 목표 지점 설정 (가운데에서 거리의 절반만큼 오른쪽)
        }
    }

    private void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position); // 플레이어와의 거리 계산

        // 플레이어 인식 범위 안에 들어오는 체크
        if (CanSeePlayer())
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

        Vector3 bulletDir = (player.position - transform.position).normalized;

        FireBullet(bulletDir);

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

        transform.rotation = Quaternion.Euler(0, 0, angle - 90); // 계산된 각도로 회전 (스프라이트 기본 방향이 위쪽이므로 -90도 보정)
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
            if (enemy.data.patrolType == PatrolType.Vertical) // 상하 패트롤 타입일 경우
            {
                patrolTarget = patrolTarget.y > patrolStart.y ? patrolStart : patrolStart + new Vector3(0, enemy.data.patrolDistance, 0); // 반대 지점으로 변경
            }

            else if (enemy.data.patrolType == PatrolType.Horizontal) // 좌우 패트롤 타입일 경우
            {
                patrolTarget = patrolTarget.x > patrolStart.x ? patrolStart : patrolStart + new Vector3(enemy.data.patrolDistance, 0, 0); // 반대 지점으로 변경
            }
        }
    }

    private bool CanSeePlayer() // 플레이어 인식 함수
    {
        Vector3 dirToPlayer = (player.position - transform.position).normalized; // 플레이어 방향 계산


        if (Vector3.Distance(transform.position, player.position) > enemy.data.detectRange) // 인식 범위 밖이면 false 반환
            return false;


        float angle = Vector3.Angle(transform.up, dirToPlayer); // 적의 현재 방향과 플레이어 방향 사이의 각도 계산
        if (angle > enemy.data.viewAngle * 0.5f) // 시야각 밖이면 false 반환
            return false;


        if (enemy.data.obstacleMask != 0) // 장애물 마스크가 설정되어 있을 경우에만 레이캐스트 검사
        {
            if (Physics.Raycast(transform.position, dirToPlayer, out RaycastHit hit, enemy.data.detectRange, enemy.data.obstacleMask)) // 장애물 마스크로 레이캐스트 검사
            {
                if (!hit.collider.CompareTag("Player")) // 레이캐스트에 맞은 것이 플레이어가 아니면 false 반환
                    return false;
            }
        }

        return true;
    }

    private void OnDrawGizmosSelected() // 시야각 디버그 그리기
    {
        if (enemy == null || enemy.data == null) return;

        Gizmos.color = Color.green;

        // 적의 현재 방향 (transform.up)
        Vector3 forward = transform.up;

        float halfAngle = enemy.data.viewAngle * 0.5f;
        float radius = enemy.data.detectRange;

        // 시야각의 양쪽 경계 벡터 계산
        Quaternion leftRot = Quaternion.Euler(0, 0, halfAngle);
        Quaternion rightRot = Quaternion.Euler(0, 0, -halfAngle);

        Vector3 leftDir = leftRot * forward;
        Vector3 rightDir = rightRot * forward;

        // 디버그 선 그리기
        Gizmos.DrawLine(transform.position, transform.position + leftDir * radius);
        Gizmos.DrawLine(transform.position, transform.position + rightDir * radius);

        // 시야 거리 원도 표시 (선택사항)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void FireBullet(Vector3 dir) // 총알 발사 함수
    {
        if (enemy.data.bulletPrefab == null)
        {
            Debug.LogWarning("인스펙터 연결하라고 제발 plz.....");
            return;
        }

        GameObject bulletObj = Instantiate(enemy.data.bulletPrefab, transform.position, Quaternion.identity);
        Bullet bullet = bulletObj.GetComponent<Bullet>();

        bullet.Initialize(dir, enemy.data.bulletSpeed, enemy.data.attackDamage, enemy.data.bulletSprite); // 총알 초기화
    }
}
