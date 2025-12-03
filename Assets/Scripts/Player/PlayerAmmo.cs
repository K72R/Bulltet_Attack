using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어가 가진 탄약을 관리하는 컴포넌트
// 권총은 무제한이라고 가정하고 관리하지 않음
// 라이플 / 샷건 탄약만 관리
// 탄창(장전된 탄) + 비축(예비 탄) 구조
public class PlayerAmmo : MonoBehaviour
{
    [Header("Rifle Ammo")]
    [Tooltip("라이플 탄창 최대 탄약 수")]
    public int rifleMagMax = 30;

    [Tooltip("현재 라이플 탄창에 남은 탄약 수")]
    public int rifleMagCurrent = 30;

    [Tooltip("라이플 비축 탄약 최대 수")]
    public int rifleReserveMax = 90;

    [Tooltip("현재 라이플 비축 탄약 수")]
    public int rifleReserveCurrent = 90;

    [Header("Shotgun Ammo")]
    [Tooltip("샷건 탄창 최대 탄약 수")]
    public int shotgunMagMax = 6;

    [Tooltip("현재 샷건 탄창에 남은 탄약 수")]
    public int shotgunMagCurrent = 6;

    [Tooltip("샷건 비축 탄약 최대 수")]
    public int shotgunReserveMax = 24;

    [Tooltip("현재 샷건 비축 탄약 수")]
    public int shotgunReserveCurrent = 24;

    // 보급 상자에서 사용할 "풀 충전" 함수
    // 라이플 / 샷건의 탄창 + 비축탄을 모두 최대치로 채움
    public void RefillAllAmmo()
    {
        rifleMagCurrent = rifleMagMax;
        rifleReserveCurrent = rifleReserveMax;

        shotgunMagCurrent = shotgunMagMax;
        shotgunReserveCurrent = shotgunReserveMax;

        Debug.Log("탄약 풀보급: 라이플/샷건 탄창+비축 전부 최대치");
    }

    // 특정 무기의 비축 탄약을 n 만큼 추가 (부분 보급용)
    public void AddReserveAmmo_Rifle(int amount)
    {
        if (amount <= 0) return;
        rifleReserveCurrent = Mathf.Clamp(rifleReserveCurrent + amount, 0, rifleReserveMax);
    }

    public void AddReserveAmmo_Shotgun(int amount)
    {
        if (amount <= 0) return;
        shotgunReserveCurrent = Mathf.Clamp(shotgunReserveCurrent + amount, 0, shotgunReserveMax);
    }

    // 발사할 때 탄약 한 발 소비 (팀원 발사 스크립트에서 호출하면 됨).
    // 무기 타입에 따라 탄창 탄약을 줄이고, 성공시 true 반환.
    public bool ConsumeFromMag(string weaponName)
    {
        switch (weaponName)
        {
            case "Rifle":
                if (rifleMagCurrent <= 0) return false;
                rifleMagCurrent--;
                return true;

            case "Shotgun":
                if (shotgunMagCurrent <= 0) return false;
                shotgunMagCurrent--;
                return true;

            default:
                // 권총은 무제한이라고 가정
                return true;
        }
    }

    // 탄창이 비었을 때 재장전할 때 호출 (예: Reload 스크립트에서).
    // 탄창을 비축탄에서 채움.
    public void Reload(string weaponName)
    {
        switch (weaponName)
        {
            case "Rifle":
                {
                    int need = rifleMagMax - rifleMagCurrent;
                    int take = Mathf.Min(need, rifleReserveCurrent);
                    rifleMagCurrent += take;
                    rifleReserveCurrent -= take;
                    break;
                }
            case "Shotgun":
                {
                    int need = shotgunMagMax - shotgunMagCurrent;
                    int take = Mathf.Min(need, shotgunReserveCurrent);
                    shotgunMagCurrent += take;
                    shotgunReserveCurrent -= take;
                    break;
                }
        }
    }
}

