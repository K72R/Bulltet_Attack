using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyType // 헷갈리지 않게 타입 정의
{
    Infantry,    // 보병
    Assault,     // 돌진병
    Sniper       // 저격병
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

    //[Header("Special")]
    //public bool useLongRangeAttack; // 저격병만 true로 사용
    //public float longRangeDamageMultiplier = 1.5f; // 저격 강화
}

