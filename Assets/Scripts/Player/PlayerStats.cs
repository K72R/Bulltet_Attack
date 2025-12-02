using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum PlayerStatus
{
    Alive,
    Dead
}

public class PlayerStats : MonoBehaviour
{
    [Header("Player Ability")]
    public PlayerStatus playerStatus;
    public int maxHealth;
    public int currentHealth;
    // 인벤토리 관련 속성 필요

    private void Awake()
    {
        currentHealth = maxHealth;
        playerStatus = PlayerStatus.Alive;
    }
    public void TakeDamages(int damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

        if(currentHealth == 0)
        {
            playerStatus = PlayerStatus.Dead;
        }
    }

    public float GetHealthPercent()
    {
        return (float)currentHealth / maxHealth;
    }

    // Idea : 플레이어 피격은 콜라이더로 피격 판정하는 것도 나쁘지 않다고 봅니다.
}
