using UnityEngine;

public class SelectUI : MonoBehaviour
{
    public PlayerWeaponController weapon;
    public PlayerAmmo ammo;

    public void SelectRifle()
    {
        weapon.hasRifle = true;
        weapon.currentWeapon = WeaponType.Rifle;

        ammo.rifleMagCurrent = ammo.rifleMagMax;
        ammo.rifleReserveCurrent = ammo.rifleReserveMax;

        //  UI 즉시 갱신
        GunUI.Instance.UpdateUI(
            "Rifle",
            ammo.rifleMagCurrent,
            ammo.rifleReserveCurrent
        );

        gameObject.SetActive(false);
    }

    public void SelectShotgun()
    {
        weapon.hasShotgun = true;
        weapon.currentWeapon = WeaponType.Shotgun;

        ammo.shotgunMagCurrent = ammo.shotgunMagMax;
        ammo.shotgunReserveCurrent = ammo.shotgunReserveMax;

        GunUI.Instance.UpdateUI(
           "Shotgun",
           ammo.shotgunMagCurrent,
           ammo.shotgunReserveCurrent
       );

        gameObject.SetActive(false);
    }
}
