using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Enemy enemy;
    private Transform player;

    private float attackCooldown = 2.5f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        // 플레이어 감지
        if (dist <= enemy.data.detectRange)
        {
            // 공격 범위 체크
            if (dist <= enemy.data.attackRange)
            {
                TryAttack();
            }
            else
            {
                MoveToPlayer();
            }
        }

        attackCooldown -= Time.deltaTime;
    }

    private void MoveToPlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * enemy.data.moveSpeed * Time.deltaTime;
    }

    private void TryAttack()
    {
        if (attackCooldown > 0f) return;

        float dmg = enemy.data.attackDamage;

        Debug.Log($" attacks! Damage: {dmg}");

        // TODO: 플레이어에게 데미지 주기

        attackCooldown = enemy.data.attackRate;
    }
}
