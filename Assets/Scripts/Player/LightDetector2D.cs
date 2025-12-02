using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;

public class SpotLightOcclusion : MonoBehaviour
{
    public Light2D spotLight;
    public LayerMask targetMask;     // Enemy 등 감지 대상
    public LayerMask obstacleMask;   // 벽/바위 같은 장애물

    void Update()
    {
        DetectObjects();
    }

    void DetectObjects()
    {
        float radius = spotLight.pointLightOuterRadius;
        float halfAngle = spotLight.pointLightOuterAngle * 0.5f;
        Vector2 forward = transform.up; // SpotLight2D의 방향

        // 반경 체크
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, targetMask);

        foreach (var hit in hits)
        {
            Transform obj = hit.transform;
            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();

            if (sr == null)
                continue;

            // 1) SpotLight 방향각 체크
            Vector2 dir = (obj.position - transform.position).normalized;
            float angle = Vector2.Angle(forward, dir);

            if (angle > halfAngle)
            {
                sr.enabled = false;
                continue;
            }

            // 2) 장애물 체크 (SpotLight → Target)
            RaycastHit2D block = Physics2D.Raycast(transform.position, dir,
                Vector2.Distance(transform.position, obj.position), obstacleMask);

            if (block.collider != null)
            {
                // 장애물에 막힘 → 빛이 닿지 않음
                sr.enabled = false;
            }
            else
            {
                // 빛이 직접 닿음 → 켜기
                sr.enabled = true;
            }
        }
    }
}
