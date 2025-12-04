using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceHandler : MonoBehaviour
{
    [Header("현재 사용중인 무기의 사운드 제어 스크립트")]
    private SoundEffect currentSound;

    private void Start()
    {
        currentSound = GetComponentInChildren<SoundEffect>();   
    }

    public void ShootSound()
    {
        currentSound.ShootSound();
    }
    public void ReloadSound()
    {
        currentSound.ReloadSound();
    }

    public void ResetCurrentSound(SoundEffect newSound)
    {
        currentSound = newSound;
    }
}
