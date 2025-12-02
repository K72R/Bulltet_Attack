using UnityEngine;
using TMPro;


public class GunUI : MonoBehaviour
{
    public TMP_Text gunNameText;
    public TMP_Text ammoText;

    public void UpdateUI(string gunName, int currentAmmo, int maxAmmo)
    {
        gunNameText.text = gunName;
        ammoText.text = currentAmmo + " / " + maxAmmo;
    }
}
