using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 맵에 떨어진 총 아이템
// 플레이어가 가까이 와서 E키를 누르면
// 해당 무기를 플레이어에게 해금시키고, 자기 자신은 사라짐

public class WeaponPickup : MonoBehaviour
{
    [Header("무기 종류 설정")]
    [Tooltip("이 아이템이 플레이어에게 어떤 무기를 해금해 줄지 설정합니다")]
    [SerializeField] private WeaponType weaponType = WeaponType.Rifle;

    [Header("상호작용 설정")]
    [Tooltip("플레이어가 이 무기를 줍기 위해 필요한 거리")]
    [SerializeField] private float interactDistance = 1.2f;

    [Tooltip("무기를 줍는 데 사용할 키 (기본: E)")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private Transform playerTransform;

    private void Start()
    {
        // Player 태그를 가진 오브젝트를 찾아 플레이어 위치를 추적
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

        // 플레이어와 무기 아이템 사이의 거리 계산
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        // 일정 거리 안으로 들어오고, E키를 눌렀을 때만 줍기 가능
        if (distance <= interactDistance)
        {
            if (Input.GetKeyDown(interactKey))
            {
                GiveWeaponToPlayer();
            }
        }
    }

    // 플레이어에게 무기를 해금시키고, 아이템을 제거합니다.
    private void GiveWeaponToPlayer()
    {
        // 플레이어에 붙어 있을 무기 관리 스크립트를 찾아서
        var weaponController = playerTransform.GetComponent<PlayerWeaponController>();

        if (weaponController != null)
        {
            // 이 아이템이 가지고 있는 weaponType을 플레이어에게 전달
            weaponController.UnlockWeapon(weaponType);
        }

        Debug.Log($"무기 획득: {weaponType}");

        // 한 번 줍고 나면 맵에서 사라지는 구조
        gameObject.SetActive(false);
        // 또는 Destroy(gameObject); 사용 가능 (완전히 삭제)
    }
}


