using UnityEngine;

public class SelectUI : MonoBehaviour
{
    public GameObject panel;

    public void SelectRifle()
    {
        GunUI.Instance.UpdateUI("Rifle", 30, 70);
        panel.SetActive(false);
    }

    public void SelectShotgun()
    {
        GunUI.Instance.UpdateUI("Shot gun", 6, 24);
        panel.SetActive(false);
    }
}
