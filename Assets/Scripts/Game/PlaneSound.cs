using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneSound : MonoBehaviour
{
    [SerializeField] private AudioSource engine;
    [SerializeField] private AudioSource waterSplash;

    // Звуки окружающей природы
    [SerializeField] private AudioSource windGround;
    [SerializeField] private AudioSource windAir;

    public void UpdateSounds(float rpm, float height)
    {
        float rpmAffect = rpm / 5000f;

        engine.volume = rpmAffect;
        engine.pitch = rpmAffect + 0.2f;

        if (height > 300f)
        {
            windGround.enabled = false;
            windAir.enabled = true;
        } else
        {
            windGround.enabled = true;
            windAir.enabled = false;
        }
    }

    public void PlayWaterSplash()
    {
        waterSplash.Play();
    }
}
