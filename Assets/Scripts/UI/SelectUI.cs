using UnityEngine;

public class SelectUI : MonoBehaviour
{
    public GameObject panel;

    public void SelectRifle()
    {
        panel.SetActive(false);
    }

    public void SelectShotgun()
    {
        panel.SetActive(false);
    }
}
