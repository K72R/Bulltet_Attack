using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    [Header("Aiming Settings")]
    private Transform firePosition;
    private LineRenderer aimingLine;
    private Vector3 rayStart;

    public float objAndRayOffset;
    public float rayLength;

    // Start is called before the first frame update
    void Start()
    {
        firePosition = this.transform;
        aimingLine = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        RayDistance();
    }

    private void RayDistance()
    {
        if (firePosition == null) return;

        rayStart = firePosition.position + transform.up * objAndRayOffset;
        aimingLine.SetPosition(0, rayStart);

        RaycastHit2D hit = Physics2D.Raycast(rayStart, transform.up, rayLength);
        Vector3 endPosition = transform.position + transform.up * rayLength;

        if (hit.collider != null && !hit.collider.TryGetComponent<Player>(out Player player))
        {
            aimingLine.SetPosition(1, hit.point);
        }
        else
        {
            aimingLine.SetPosition(1, endPosition);
        }

        
    }

    public void Attack(int damage)
    {
        if (firePosition == null || rayStart == null) return;

        RaycastHit2D hit = Physics2D.Raycast(rayStart, transform.up, rayLength);

        if (hit.collider != null)
        {
            if(hit.collider.TryGetComponent<Enemy>(out Enemy enemyStats))
            {
                enemyStats.TakeDamage(damage);
                Debug.Log("Hit Enemy");
            }
            else
            {
                Debug.Log("Hit Something Else");
            }
        }
        else
        {
            Debug.Log("Miss");
        }
    }
    public void SetFirePoint(Transform newFirePoint)
    {
        firePosition = newFirePoint;
    }
}
