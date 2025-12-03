using UnityEngine;

public class GunUITest : MonoBehaviour
{
    public string gunName = "Rifle";

    public int magazineSize = 30;      // 장전 시 탄창 최대 수
    public int currentMagazine = 30;   // 현재 탄창
    public int reserveAmmo = 70;       // 보유 탄약

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        // 왼쪽 클릭 = 발사
        if (Input.GetMouseButtonDown(0))
        {
            if (currentMagazine > 0)
            {
                currentMagazine--;
                UpdateUI();
            }
        }

        // R = 장전
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    private void Reload()
    {
        int needed = magazineSize - currentMagazine; // 탄창에 필요한 탄약 수

        if (needed > 0 && reserveAmmo > 0)
        {
            int useAmmo = Mathf.Min(needed, reserveAmmo);
            currentMagazine += useAmmo;
            reserveAmmo -= useAmmo;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        GunUI.Instance.UpdateUI(gunName, currentMagazine, reserveAmmo);
    }
}
