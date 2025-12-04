using UnityEngine;

public class GunUITest : MonoBehaviour
{
    private PlayerAmmo ammo;
    private PlayerWeaponController weapon;

    private float reloadDelay = 0.0f;   // 장전 대기 시간(애니메이션 시간)

    // 최근 상태 캐시 (변화가 있을 때만 UI 갱신)
    private string lastWeapon = "";
    private int lastMag = -1;
    private int lastReserve = -1;

    private void Start()
    {
        ammo = GetComponent<PlayerAmmo>();
        weapon = GetComponent<PlayerWeaponController>();

        if (ammo == null || weapon == null)
        {
            return;
        }

        RefreshUIImmediate();
    }

    private void Update()
    {
        // 무기 바뀌면 UI 갱신
        if (Input.GetKeyDown(KeyCode.Alpha1) ||
            Input.GetKeyDown(KeyCode.Alpha2) ||
            Input.GetKeyDown(KeyCode.Alpha3))
        {
            RefreshUIImmediate();
        }

        // 장전 시작
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartReload();
        }

        // 장전 대기 시간이 끝났다면 실제 장전 적용
        if (reloadDelay > 0f)
        {
            reloadDelay -= Time.unscaledDeltaTime;
            if (reloadDelay <= 0f)
            {
                ApplyReload();
            }
        }

        HandleAutoSwitch();

        PollAndRefreshUI();
    }

    private void PollAndRefreshUI()
    {
        if (weapon == null || ammo == null) return;

        string w = weapon.currentWeapon.ToString();
        int mag = 0;
        int reserve = 0;

        if (w == "Pistol")
        {
            mag = 999; reserve = 999;
        }
        else if (w == "Rifle")
        {
            mag = ammo.rifleMagCurrent;
            reserve = ammo.rifleReserveCurrent;
        }
        else if (w == "Shotgun")
        {
            mag = ammo.shotgunMagCurrent;
            reserve = ammo.shotgunReserveCurrent;
        }

        if (w != lastWeapon || mag != lastMag || reserve != lastReserve)
        {
            GunUI.Instance.UpdateUI(w, mag, reserve);
            lastWeapon = w;
            lastMag = mag;
            lastReserve = reserve;
        }
    }

    // 즉시 UI 강제 갱신
    private void RefreshUIImmediate()
    {
        lastWeapon = "";
        lastMag = -1;
        lastReserve = -1;
        PollAndRefreshUI();
    }

    // 장전
    private void StartReload()
    {
        string w = weapon != null ? weapon.currentWeapon.ToString() : "Pistol";

        if (w == "Pistol") return;

        reloadDelay = 1.0f;
    }

    private void ApplyReload()
    {
        if (weapon == null || ammo == null) return;

        string w = weapon.currentWeapon.ToString();

        ammo.Reload(w);

        RefreshUIImmediate();
    }

    // 권총 자동 변경
    private void HandleAutoSwitch()
    {
        if (weapon == null || ammo == null) return;

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
            GunUI.Instance.UpdateUI("Pistol", 999, 999);
        }
    }
}
