using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어가 몸으로 밀어서 움직일 수 있는 엄폐물.
// - Rigidbody2D 물리에 의해 움직임
// - 필요하면 X 또는 Y 축으로만 움직이게 제한 가능
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PushableCover : MonoBehaviour
{
    [Header("이동 축 제한 옵션")]
    [Tooltip("true면 특정 축으로만 움직이게 제한합니다. (예: 좌우로만 또는 위아래로만)")]
    [SerializeField] private bool useAxisLimit = false; // 축 제한 기능을 쓸지 여부

    [Tooltip("X축 이동을 막을지 여부 (true면 X속도를 0으로 고정 → 위/아래로만 움직임)")]
    [SerializeField] private bool lockX = false;        // 가로 방향 이동 제한 여부

    [Tooltip("Y축 이동을 막을지 여부 (true면 Y속도를 0으로 고정 → 좌/우로만 움직임)")]
    [SerializeField] private bool lockY = false;        // 세로 방향 이동 제한 여부

    // 실제 물리 연산을 담당하는 컴포넌트 (질량, 마찰 등)
    private Rigidbody2D rb;

    private void Awake()
    {
        // 이 상자에 붙어 있는 Rigidbody2D를 찾아서 보관
        rb = GetComponent<Rigidbody2D>();

        // 탑뷰 상자라서 넘어지거나 회전하지 않게 고정
        rb.freezeRotation = true;
        // 위/아래로 떨어지는 중력은 사용하지 않음 (탑뷰 게임이기 때문)
        rb.gravityScale = 0f;
    }

    private void FixedUpdate()
    {
        // 축 제한 기능을 사용하지 않으면, 순수 물리 동작만 사용
        if (!useAxisLimit) return;

        // 현재 상자의 속도를 가져옴
        Vector2 v = rb.velocity;

        // lockX가 켜져 있으면 X축 속도를 0으로 만들어 가로 이동을 막음
        if (lockX)
        {
            v.x = 0f;
        }

        // lockY가 켜져 있으면 Y축 속도를 0으로 만들어 세로 이동을 막음
        if (lockY)
        {
            v.y = 0f;
        }

        // 수정한 속도를 다시 Rigidbody2D에 적용
        rb.velocity = v;
    }
}