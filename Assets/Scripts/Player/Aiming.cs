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
        aimingLine.SetPosition(0, firePosition.position + transform.up * objAndRayOffset);

        RaycastHit2D hit = Physics2D.Raycast(firePosition.position, transform.up, rayLength);
        Vector3 endPosition = transform.position + transform.up * rayLength;

        if (hit.collider != null)
        {
            aimingLine.SetPosition(1, hit.point);
        }
        else
        {
            aimingLine.SetPosition(1, endPosition);
        }
    }

    public void Attack()
    {
        RaycastHit2D hit = Physics2D.Raycast(firePosition.position, transform.up, rayLength);

        if (hit.collider != null)
        {
            Debug.Log($"Hit : {hit.collider.name}");
        }
        else
        {
            Debug.Log("Miss");
        }
    }
}
