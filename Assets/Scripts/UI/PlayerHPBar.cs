using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    public float curValue;
    public float maxValue;
    public float startValue;
    public Image uiBar;

    private void Start()
    {
        curValue = startValue;
        UpdateUI();
    }


    public void Add(float amount)
    {
        curValue = Mathf.Min(curValue + amount, maxValue);
        UpdateUI();
    }

    public void Subtract(float amount)
    {
        curValue = Mathf.Max(curValue - amount, 0.0f);
        UpdateUI();
    }

    public void SetHealth(float value)
    {
        curValue = Mathf.Clamp(value, 0, maxValue);
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (uiBar != null)
            uiBar.fillAmount = curValue / maxValue;
    }
}
