using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerVisionLightController : MonoBehaviour
{
    [Header("플레이어 시야 라이트")]
    public Light2D fovLight;

    [Header("레이어 마스크")]
    public LayerMask enemyMask;
    public LayerMask obstacleMask;

    private void Update()
    {
        DetectEnemies();
    }

    private void DetectEnemies()
    {
        float radius = fovLight.pointLightOuterRadius;
        float halfAngle = fovLight.pointLightOuterAngle * 0.5f;
        Vector2 forward = transform.up;

        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            transform.position, radius, enemyMask);

        foreach (var enemy in enemies)
        {
            Light2D enemyLight = enemy.GetComponentInChildren<Light2D>(true);

            if (enemyLight == null)
                continue;

            Vector2 dir = (enemy.transform.position - transform.position).normalized;
            float angle = Vector2.Angle(forward, dir);

            // 플레이어 시야각 밖이면 Light OFF
            if (angle > halfAngle)
            {
                enemyLight.enabled = false;
                continue;
            }

            // 장애물에 가리면 OFF
            if (Physics2D.Raycast(transform.position, dir,
                Vector2.Distance(transform.position, enemy.transform.position),
                obstacleMask))
            {
                enemyLight.enabled = false;
            }
            else
            {
                // 플레이어에게 보이면 ON
                enemyLight.enabled = true;
            }
        }
    }
}
