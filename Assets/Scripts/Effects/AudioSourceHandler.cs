using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceHandler : MonoBehaviour
{
    [Header("현재 사용중인 무기의 사운드 제어 스크립트")]
    private SoundEffect currentSound;
    [Header("플레이어 자체의 효과음")]
    public AudioClip damaged;
    public AudioClip death;
    private AudioSource mainSound;

    private void Start()
    {
        mainSound = GetComponent<AudioSource>();
        currentSound = GetComponentInChildren<SoundEffect>();   
    }

    public void HurtSound()
    {
        mainSound.PlayOneShot(damaged);
    }

    public void DeathSound()
    {
        mainSound.PlayOneShot(death);
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
