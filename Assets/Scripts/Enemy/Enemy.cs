using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData data;   // Inspector에서 SO를 넣어두면 된다.

    private float currentHP;  // 현재 체력입니다

    private void Awake()
    {
        currentHP = data.maxHP;     // 시작 시 현재 체력을 최대 체력으로 초기화 시킵니다
    }

    public float GetMoveSpeed() => data.moveSpeed; // EnemyData에서 만든 SO데이터를 기반하여 이동속도를 가지고 옵니다 (인스펙터에서 조정 가능)
    public float GetDamage() => data.attackDamage; // EnemyData에서 만든 SO데이터를 기반하여 공격력을 가지고 옵니다 (인스펙터에서 조정 가능)

    public void TakeDamage(float dmg) // 공격 받을 때 호출되는 함수
    {
        currentHP -= dmg; // 데미지만큼 현재 체력을 깎습니다

        if (currentHP <= 0) // 체력이 0 이하가 되면
            Die(); // 죽습니다
    }

    private void Die() // 죽는 함수
    {
        Destroy(gameObject); // 죽을 시 오브젝트를 파괴합니다
    }
}
