using UnityEngine;

public class SelectUI : MonoBehaviour
{
    public PlayerWeaponController weapon;
    public PlayerAmmo ammo;

    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    public void SelectRifle()
    {
        weapon.UnlockWeapon(WeaponType.Rifle); weapon.currentWeapon = WeaponType.Rifle;
        weapon.HandleWeaponSelectInput(WeaponType.Rifle);

        ammo.rifleMagCurrent = ammo.rifleMagMax;
        ammo.rifleReserveCurrent = ammo.rifleReserveMax;

        //  UI 즉시 갱신
        GunUI.Instance.UpdateUI(
            "Rifle",
            ammo.rifleMagCurrent,
            ammo.rifleReserveCurrent
        );

        ClosePanel();
    }

    public void SelectShotgun()
    {
        weapon.UnlockWeapon(WeaponType.Shotgun); weapon.currentWeapon = WeaponType.Shotgun;
        weapon.HandleWeaponSelectInput(WeaponType.Shotgun);

        ammo.shotgunMagCurrent = ammo.shotgunMagMax;
        ammo.shotgunReserveCurrent = ammo.shotgunReserveMax;

        GunUI.Instance.UpdateUI(
           "Shotgun",
           ammo.shotgunMagCurrent,
           ammo.shotgunReserveCurrent
       );

        ClosePanel();
    }

    private void ClosePanel()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        gameObject.SetActive(false);
    }
}
