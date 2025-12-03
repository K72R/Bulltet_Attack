using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어가 상자 근처에서 E키를 눌러
// 탄약을 보급받을 수 있는 보급 상자
// 한 번만 사용되게 할 수도 있고
// 여러 번 재사용 가능하게 설정할 수도 있음
public class SupplyBox : MonoBehaviour
{
    [Header("상자 시각 설정")]
    [Tooltip("닫혀 있을 때 보여줄 상자 스프라이트")]
    [SerializeField] private Sprite closedSprite;

    [Tooltip("열렸을 때 보여줄 상자 스프라이트")]
    [SerializeField] private Sprite openedSprite;

    [Header("상호작용 설정")]
    [Tooltip("플레이어로 인식할 레이어 (예: Player)")]
    [SerializeField] private LayerMask playerLayer;

    [Tooltip("플레이어가 상자와 상호작용할 수 있는 거리")]
    [SerializeField] private float interactDistance = 1.0f;

    [Tooltip("상호작용에 사용할 키 (기본: E)")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("보급 설정")]
    [Tooltip("이 상자를 여러 번 사용할 수 있는지 여부 (true면 재사용 가능)")]
    [SerializeField] private bool reusable = false;

    private bool isOpened = false;          // 한 번이라도 열린 적 있는지 여부
    private SpriteRenderer spriteRenderer;  // 상자 그림을 바꾸기 위한 렌더러
    private Transform playerTransform;      // 플레이어 위치 추적

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 처음에는 닫힌 상자 모습으로 시작
        if (closedSprite != null)
        {
            spriteRenderer.sprite = closedSprite;
        }
    }

    private void Start()
    {
        // Player 태그를 가진 오브젝트를 찾아서 플레이어로 사용
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

        // 이미 한 번 열렸고, 재사용이 불가능하면 더 이상 작동하지 않음
        if (isOpened && !reusable) return;

        float distance = Vector2.Distance(transform.position, playerTransform.position);

        // 플레이어가 상자 근처에 있을 때만 상호작용 가능
        if (distance <= interactDistance)
        {
            if (Input.GetKeyDown(interactKey))
            {
                OpenAndSupply();
            }
        }
    }

    // 상자를 열고, 플레이어에게 탄약을 보급하는 함수.
    // 실제 탄약 회복 로직은 플레이어/무기 시스템과 연결해야 함.
    private void OpenAndSupply()
    {
        // 상자 상태 플래그 설정
        isOpened = true;

        // 상자 스프라이트를 열린 상태로 변경
        if (openedSprite != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = openedSprite;
        }

        // 실제 탄약 회복 로직 구현
        // 실제 프로젝트 코드에 맞게 수정 필요
        var playerAmmo = playerTransform.GetComponent<PlayerAmmo>();
        if (playerAmmo != null)
        {
            playerAmmo.RefillAllAmmo();
        }

        Debug.Log("보급 상자 사용: 플레이어 탄약 보급 (일단 구현하고 나중에 코드넣어서 세부적으로 나누면됨)");

        // 재사용 불가 상자라면, 상호작용을 막기 위한 추가 처리 가능
        // 콜라이더 비활성화, 상자 비활성화 등
        if (!reusable)
        {
            //// 콜라이더를 꺼서 더 이상 상호작용되지 않게 할 수도 있음
            //Collider2D col = GetComponent<Collider2D>();
            //if (col != null)
            //{
            //    col.enabled = false;
            //}

            // 상자 열면 바로 사라지게
            gameObject.SetActive(false);
            // 만약 짧게 열린 모션을 보여주고 싶으면
            // StartCoroutine(DisappearAfterDelay(0.2f)); 이런 식으로 코루틴 쓰면 됨.
        }
    }
}