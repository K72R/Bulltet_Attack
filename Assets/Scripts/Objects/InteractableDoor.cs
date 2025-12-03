using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;

// 플레이어가 근처에서 상호작용(E키)을 눌러
// 문을 열고 닫을 수 있게 해 주는 스크립트
// - 닫힘 상태: 콜라이더 ON → 통과 불가
// - 열림 상태: 콜라이더 OFF → 통과 가능
// - 스프라이트를 바꿔서 열린 문 / 닫힌 문을 시각적으로 구분
[RequireComponent(typeof(BoxCollider2D))]
public class InteractableDoor : MonoBehaviour
{
    [Header("문 기본 설정")]
    [Tooltip("플레이어가 문과 겹칠 수 있도록 IsTrigger = true인 콜라이더를 사용합니다.")]
    [SerializeField] private BoxCollider2D doorCollider; // 실제로 막아주는 콜라이더

    [Tooltip("문이 닫혀 있을 때 보여줄 스프라이트")]
    [SerializeField] private Sprite closedSprite;

    [Tooltip("문이 열려 있을 때 보여줄 스프라이트")]
    [SerializeField] private Sprite openedSprite;

    [Header("상호작용 설정")]
    [Tooltip("플레이어로 인식할 레이어 (예: Player)")]
    [SerializeField] private LayerMask playerLayer;

    [Tooltip("플레이어가 문과 상호작용할 수 있는 거리")]
    [SerializeField] private float interactDistance = 1.0f;

    [Tooltip("상호작용에 사용할 키 (기본: E)")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("라이트/시야 설정")]
    [SerializeField] private ShadowCaster2D shadowCaster; // 문이 라이트를 막는 컴포넌트

    private SpriteRenderer spriteRenderer; // 문 그림을 바꾸기 위한 렌더러
    private bool isOpen = false;           // 현재 문이 열려 있는지 여부

    private Transform playerTransform;     // 간단히 플레이어 위치를 추적

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // doorCollider를 인스펙터에서 지정 안 했으면 자신 것 자동 할당
        if (doorCollider == null)
        {
            doorCollider = GetComponent<BoxCollider2D>();
        }

        // 처음에는 문을 닫힌 상태로 시작
        SetDoorState(false);
    }

    private void Start()
    {
        // 아주 단순하게, 씬에서 Player 태그를 가진 오브젝트를 찾아서 사용
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

        // 플레이어와 문의 거리 계산
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        // 플레이어가 문 근처에 있을 때만 상호작용 가능
        if (distance <= interactDistance)
        {
            // E키를 눌렀을 때 열린/닫힌 상태를 토글
            if (Input.GetKeyDown(interactKey))
            {
                ToggleDoor();
            }
        }
    }

    // 문 상태를 반대로 전환 (열려 있으면 닫고, 닫혀 있으면 여는 함수)
    private void ToggleDoor()
    {
        SetDoorState(!isOpen);
    }

    // 실제로 문 상태를 변경하는 함수.
    // isOpen == true  → 문 열림 (콜라이더 OFF, 열린 스프라이트)
    // isOpen == false → 문 닫힘 (콜라이더 ON, 닫힌 스프라이트)
    private void SetDoorState(bool open)
    {
        isOpen = open;

        // 막아주는 콜라이더 ON/OFF
        if (doorCollider != null)
        {
            doorCollider.enabled = !isOpen;
        }

        if (shadowCaster != null)
        {
            shadowCaster.enabled = !isOpen;  // 닫힘: true(막음) / 열림: false(안 막음)
        }

        // 문 그림 변경
        if (spriteRenderer != null)
        {
            if (isOpen && openedSprite != null)
            {
                spriteRenderer.sprite = openedSprite;
            }
            else if (!isOpen && closedSprite != null)
            {
                spriteRenderer.sprite = closedSprite;
            }
        }

        // 나중에 문 열릴 때/닫힐 때 효과음, 파티클 등을 추가할 수 있음.
        // 예시) AudioSource.PlayOneShot(openSound);
    }
}