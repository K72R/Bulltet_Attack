using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemHandler : MonoBehaviour
{
    private ParticleSystem muzzleFlashEffect;

    private void Start()
    {
        Transform firePosition = transform.Find("PlayerObj/FirePosition");
        if (firePosition != null)
        {
            muzzleFlashEffect = firePosition.GetComponentInChildren<ParticleSystem>();
        }
    }

    public void FireEffectsOn()
    {
        if (muzzleFlashEffect == null) return;

        muzzleFlashEffect?.Play();
    }

    public void EffectReset(Transform newFirePosition)
    {
        muzzleFlashEffect = newFirePosition.GetComponentInChildren<ParticleSystem>();
    }
}
