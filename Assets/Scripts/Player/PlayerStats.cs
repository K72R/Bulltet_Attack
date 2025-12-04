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

    private AudioSourceHandler audio;

    private void Awake()
    {
        audio = GetComponent<AudioSourceHandler>();
        currentHealth = maxHealth;
        playerStatus = PlayerStatus.Alive;
    }
    public void TakeDamages(int damage)
    {
        audio.HurtSound();
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

        if(currentHealth == 0)
        {
            audio.DeathSound();
            playerStatus = PlayerStatus.Dead;
        }
    }

    public float GetHealthPercent()
    {
        return (float)currentHealth / maxHealth;
    }
}
