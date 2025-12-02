using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    private SpriteRenderer spriteRenderer; // 플레이어 스프라이트 렌더러
    private bool isLeftHanded = false; // 플레이어가 무기를 어느 손에 들고 있는지 여부
    private Transform firePosition;

    [Header("Movement Settings")]
    public float moveSpeed; // 이동 속도
    private Rigidbody2D rb; // 플레이어 리지드바디
    private Vector2 curMovementInput; // 현재 이동 입력 벡터 (W,A,S,D 입력)

    [Header("Look Settings")]
    public Vector3 mouseDelta; // 현재 마우스 커서의 위치값

    [Header("Combat Settings")]
    private Aiming aiming;

    [Header("AnimationControll")]
    private AnimationHandler animationHandler; // 애니메이션 핸들러

    [Header("PlayerPosition")]
    public Vector2 playerPosition; // 플레이어의 실시간 위치

    private Camera cam; // 마우스 커서의 좌표를 얻기 위한 카메라 참조

    private void Awake()
    {
        playerPosition = Vector2.zero;
        animationHandler = GetComponent<AnimationHandler>();
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    private void Start()
    {
        firePosition = this.transform.Find("PlayerObj/FirePosition");
        aiming = firePosition.GetComponent<Aiming>();
        // 자식 오브젝트의 스프라이트 렌더러 컴포넌트 가져오기
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.flipY = isLeftHanded;
        Cursor.visible = false;
    }
    private void Update()
    {
        IsChangeHand();
        // 매 프레임마다 플레이어의 위치 업데이트
        playerPosition = transform.GetChild(0).position;
        GetInput();
        PlayerRotate();
        HandleCombatInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// 플레이어 이동
    /// </summary>
    private void Move()
    {
        Vector2 move = transform.right * curMovementInput.x + transform.up * curMovementInput.y;
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);

        animationHandler.Move(curMovementInput * moveSpeed);
    }

    /// <summary>
    /// 공격 처리
    /// </summary>
    private void HandleCombatInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            animationHandler.Shoot();
            Debug.Log("공격");
            aiming.Attack();
        }
    }

    /// <summary>
    /// 플레이어 회전
    /// </summary>
    private void PlayerRotate()
    {
        float spriteBaseOffset = -90f;
        Vector2 lookDir = ((Vector2)mouseDelta - playerPosition);

        if (lookDir.magnitude < 0.9f)
        {
            lookDir = Vector2.zero;
        }

        float angle = Mathf.Atan2(lookDir.y , lookDir.x ) * Mathf.Rad2Deg;

        rb.rotation = angle + spriteBaseOffset;
    }

    /// <summary>
    /// 입력 처리
    /// </summary>
    private void GetInput()
    {
        curMovementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = Mathf.Abs(cam.transform.position.z);

        mouseDelta = cam.ScreenToWorldPoint(mouseScreen);
    }

    private void IsChangeHand()
    {
        if (spriteRenderer == null) return;

        spriteRenderer.flipY = isLeftHanded;
        firePosition.localPosition = new Vector3(firePosition.localPosition.x, isLeftHanded ? 0.53f : -0.53f, firePosition.localPosition.z);
    }

    /// <summary>
    /// 손 바꾸기 커맨드 입력시 호출
    /// </summary>
    public void HandChangeButtonOnPress()
    {
        isLeftHanded = !isLeftHanded;
    }
}
