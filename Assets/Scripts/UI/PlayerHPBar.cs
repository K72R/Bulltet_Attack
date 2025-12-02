using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    public Image uiBar;

    private PlayerStats playerStats;

    private void Start()
    {
        // PlayerStats 찾아오기
        playerStats = FindObjectOfType<PlayerStats>();
    }

    private void Update()
    {
        if (playerStats != null)
        {
            uiBar.fillAmount = playerStats.GetHealthPercent();
        }
    }
}
