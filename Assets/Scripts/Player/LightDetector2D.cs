using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;

public class LightDetector2D : MonoBehaviour
{
    public Light2D spotLight;  // Spot Light 2D 할당
    public LayerMask targetMask;

    private List<SpriteRenderer> detected = new List<SpriteRenderer>();

    void Update()
    {
        foreach(var c in detected)
        {
            c.enabled = false; // 끄기
        }

        DetectObjects();
    }

    private void DetectObjects()
    {
        detected.Clear();

        float radius = spotLight.pointLightOuterRadius;

        // 반경 안의 모든 콜라이더 가져오기
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, targetMask);

        foreach (var hit in hits)
        {
            Transform obj = hit.transform;

            Vector2 dir = (obj.position - transform.position).normalized;
            Vector2 forward = transform.up; // SpotLight2D는 Up 방향이 빛 방향

            float angle = Vector2.Angle(forward, dir);

            // 스포트라이트의 각도
            float halfAngle = spotLight.pointLightOuterAngle * 0.5f;

            if (angle <= halfAngle)
            {
                // 빛에 닿은 것으로 처리
                SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.enabled = true; // 켜주기
                    detected.Add(sr);
                }
            }
        }
    }
}