using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed;
    private Rigidbody2D rb;
    private Vector2 curMovementInput;

    [Header("Look Settings")]
    public Vector2 mouseDelta;

    [Header("AnimationControll")]
    private AnimationHandler animationHandler;

    [Header("PlayerPosition")]
    public Vector2 playerPosition;

    private Camera cam;

    private void Awake()
    {
        playerPosition = Vector2.zero;
        animationHandler = GetComponent<AnimationHandler>();
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    private void Update()
    {
        playerPosition = this.transform.position;
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
        }
    }

    /// <summary>
    /// 플레이어 회전
    /// </summary>
    private void PlayerRotate()
    {
        Vector2 lookDir = mouseDelta - rb.position;
        float angle = Mathf.Atan2(lookDir.y , lookDir.x ) * Mathf.Rad2Deg - 90f;

        rb.rotation = angle;
    }

    /// <summary>
    /// 입력 처리
    /// </summary>
    private void GetInput()
    {
        curMovementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        mouseDelta = cam.ScreenToWorldPoint(Input.mousePosition);
    }
}
