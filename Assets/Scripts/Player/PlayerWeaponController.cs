using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어가 어떤 무기를 해금했는지 / 사용할 수 있는지 관리하는 스크립트
// 실제 발사는 팀원 스크립트에서 처리
// 이 스크립트는 "해금 상태"만 관리하는 용도로 사용
public class PlayerWeaponController : MonoBehaviour
{
    [Header("해금된 무기 상태")]
    public bool hasRifle = false;
    public bool hasShotgun = false;

    // 맵에 떨어진 무기 아이템이 이 함수를 호출
    // 해당 무기를 해금시킵니다(얻어서 쓰는것)
    public void UnlockWeapon(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.Rifle:
                hasRifle = true;
                Debug.Log("라이플 획득(사용 가능)");
                break;

            case WeaponType.Shotgun:
                hasShotgun = true;
                Debug.Log("샷건 획득(사용 가능");
                break;
        }

        // 여기서 실제 무기 교체/장착 로직을 넣을 수 있습니당
        // 예: 현재 무기를 라이플로 바꾸거나, 무기 선택 UI 갱신 등
    }

    [Header("현재 선택된 무기")]
    [Tooltip("현재 플레이어가 들고 있는 무기 타입")]
    public WeaponType currentWeapon = WeaponType.Pistol;

    private void Start()
    {
        // 기본 무기는 권총이라고 가정
        currentWeapon = WeaponType.Pistol;
    }

    // 무기 선택 입력
    // 지금은 Debug.Log로만 어떤 무기가 선택됐는지 확인하는 용도
    // 총/모션을 바꾸는건 다른 스크립트에서 처리해야함
    // currentWeapon 값을 보고 처리하면 됨
    public void HandleWeaponSelectInput(WeaponType type)
    {
        if (type == currentWeapon) return;

        switch(type)
        {
            case WeaponType.Pistol:
                currentWeapon = WeaponType.Pistol;
                break;
            case WeaponType.Rifle:
                currentWeapon = WeaponType.Rifle;
                break;
            case WeaponType.Shotgun:
                currentWeapon = WeaponType.Shotgun;
                break;
        }
        //// 1번: 권총 (기본 무기, 항상 사용 가능)
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    
        //    Debug.Log("무기 변경: 권총");
        //}

        //// 2번: 라이플 (해금됐을 때만 선택 가능)
        //if (Input.GetKeyDown(KeyCode.Alpha2) && hasRifle)
        //{
        //    
        //    Debug.Log("무기 변경: 라이플");
        //}

        //// 3번: 샷건 (해금됐을 때만 선택 가능)
        //if (Input.GetKeyDown(KeyCode.Alpha3) && hasShotgun)
        //{
        //    
        //    Debug.Log("무기 변경: 샷건");
        //}

        PlayerController controller = GetComponent<PlayerController>();
        controller.SendNewSkin(currentWeapon);
    }


}
