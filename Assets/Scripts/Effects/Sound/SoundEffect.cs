using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    [Header ("SFX Sounds")]
    public AudioClip reloadSound;
    public AudioClip shootSound;

    [Header ("Component Data")]
    private AudioSource audioSource;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ReloadSound()
    {
        audioSource.PlayOneShot(reloadSound);
    }

    public void ShootSound()
    {
        audioSource.PlayOneShot(shootSound);
    }
}
