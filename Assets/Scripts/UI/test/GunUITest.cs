using UnityEngine;

public class GunUITest : MonoBehaviour
{
    private PlayerAmmo ammo;
    private PlayerWeaponController weapon;

    private float reloadDelay = 0.0f;   // 장전 대기 시간(애니메이션 시간)

    private void Start()
    {
        ammo = GetComponent<PlayerAmmo>();
        weapon = GetComponent<PlayerWeaponController>();

        UpdateUI();
    }

    private void Update()
    {
        // 무기 바뀌면 UI 갱신
        if (Input.GetKeyDown(KeyCode.Alpha1) ||
            Input.GetKeyDown(KeyCode.Alpha2) ||
            Input.GetKeyDown(KeyCode.Alpha3))
        {
            UpdateUI();
        }

        // 발사 시 탄약 UI 갱신 (실제 발사 스크립트에서 ConsumeFromMag 호출한다고 가정)
        if (Input.GetMouseButtonDown(0))
        {
            TryFire();
        }

        // R 키 → 장전 시작
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartReload();
        }

        // 장전 대기 시간이 끝났다면 실제 장전 적용
        if (reloadDelay > 0f)
        {
            reloadDelay -= Time.deltaTime;

            if (reloadDelay <= 0f)
            {
                ApplyReload();
            }
        }
    }

    // 발사 처리
    private void TryFire()
    {
        string w = weapon.currentWeapon.ToString();

        if (w == "Pistol")
        {
            // 권총은 무한탄
            GunUI.Instance.UpdateUI("Pistol", 999, 999);
            return;
        }

        bool ok = ammo.ConsumeFromMag(w);

        if (!ok)
        {
            Debug.Log("탄창 비었음 → 장전 필요");
            return;
        }

        UpdateUI();

        // 탄약 0인데 예비탄도 0 → 자동 권총 변경
        HandleAutoSwitch();
    }

    // 장전
    private void StartReload()
    {
        string w = weapon.currentWeapon.ToString();

        if (w == "Pistol") return;

        Debug.Log("장전 애니메이션 재생");
        reloadDelay = 1.0f; // 여기에 실제 재장전 애니메이션 길이 넣으면 됨
    }

    private void ApplyReload()
    {
        string w = weapon.currentWeapon.ToString();

        ammo.Reload(w);

        Debug.Log("장전 완료");
        UpdateUI();
    }

    // 탄약 0 → 권총 자동 변경
    private void HandleAutoSwitch()
    {
        string w = weapon.currentWeapon.ToString();

        if (w == "Pistol") return;

        int mag = 0;
        int reserve = 0;

        if (w == "Rifle")
        {
            mag = ammo.rifleMagCurrent;
            reserve = ammo.rifleReserveCurrent;
        }
        else if (w == "Shotgun")
        {
            mag = ammo.shotgunMagCurrent;
            reserve = ammo.shotgunReserveCurrent;
        }

        // 탄창이 0이고 예비탄도 0이면 자동 권총 전환
        if (mag <= 0 && reserve <= 0)
        {
            weapon.currentWeapon = WeaponType.Pistol;

            // 무한탄 UI: 탄창 12, 보유 무한
            GunUI.Instance.UpdateUI("Pistol", 999, 999);
            Debug.Log("탄약 없음 → 권총으로 자동 전환");
        }
    }

    // UI 업데이트
    private void UpdateUI()
    {
        string w = weapon.currentWeapon.ToString();

        if (w == "Pistol")
        {
            GunUI.Instance.UpdateUI("Pistol", 999, 999);
        }
        else if (w == "Rifle")
        {
            GunUI.Instance.UpdateUI("Rifle", ammo.rifleMagCurrent, ammo.rifleReserveCurrent);
        }
        else if (w == "Shotgun")
        {
            GunUI.Instance.UpdateUI("Shotgun", ammo.shotgunMagCurrent, ammo.shotgunReserveCurrent);
        }
    }
}
