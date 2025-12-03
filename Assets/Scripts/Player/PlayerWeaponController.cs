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
                Debug.Log("라이플 해금됨");
                break;

            case WeaponType.Shotgun:
                hasShotgun = true;
                Debug.Log("샷건 해금됨");
                break;
        }

        // 여기서 실제 무기 교체/장착 로직을 넣을 수 있습니당
        // 예: 현재 무기를 라이플로 바꾸거나, 무기 선택 UI 갱신 등
    }
}
