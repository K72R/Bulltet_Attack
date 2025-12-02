using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player 관련 스크립트 통합 관리
/// </summary>
public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerStats stats;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        stats = GetComponent<PlayerStats>();
    }

    private void LateUpdate()
    {
        if(stats.playerStatus == PlayerStatus.Dead)
        {
            PlayerIsDie();
        }
    }

    private void PlayerIsDie()
    {
        Destroy(this.gameObject);
    }
}
