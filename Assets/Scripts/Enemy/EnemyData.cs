using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyType // 헷갈리지 않게 타입 정의
{
    Infantry,    // 보병
    Assault,     // 돌진병
    Sniper       // 저격병
}

public enum PatrolType // 패트롤 타입 정의
{
    None,   // 패트롤 안 함
    Vertical,   // 상 하 패트롤
    Horizontal  // 좌 우 패트롤
}

[CreateAssetMenu(fileName = "EnemyData", menuName = "Game/Enemy Data")] // SO 생성 메뉴 추가
public class EnemyData : ScriptableObject
{
    [Header("기본 설정")]
    public EnemyType enemyType;
    // public string enemyName;  // 적 이름이 필요하면 사용 예정
    public Sprite enemySprite;   // 적 이미지

    [Header("적 스탯")]
    public float maxHP = 50f; // 최대 체력
    public float moveSpeed = 3f; // 이동 속도
    public float attackDamage = 10f; // 공격력
    public float attackRate = 1f; // 공격 속도 (초당 공격 횟수)

    [Header("인식범위 및 공격사거리")]
    public float detectRange = 6f;     // 플레이어 감지 범위
    public float attackRange = 4f;   // 공격 범위

    [Header("패트롤 세팅")]
    public PatrolType patrolType;
    public float patrolDistance = 4f;  // 왕복 거리
    public float patrolSpeed = 1f;   // 순찰 속도 (인식 했을 때보다 느리게)
}

