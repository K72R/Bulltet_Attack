using UnityEngine;
using TMPro;


public class GunUI : MonoBehaviour
{
    public static GunUI Instance;

    public TMP_Text gunNameText;
    public TMP_Text ammoText;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateUI(string gunName, int currentAmmo, int maxAmmo)
    {
        gunNameText.text = gunName;
        ammoText.text = currentAmmo + " / " + maxAmmo;
    }
}
