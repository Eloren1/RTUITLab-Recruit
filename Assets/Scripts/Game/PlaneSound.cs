using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneSound : MonoBehaviour
{
    [Header("Одиночные звуки")]
    [SerializeField] private AudioSource waterSplash;
    [SerializeField] private AudioSource collectedSound;
    [SerializeField] private AudioSource explosion;

    [Header("Повторяющиеся звуки")]
    [SerializeField] private AudioSource engine;
    [SerializeField] private AudioSource windGround;
    [SerializeField] private AudioSource windAir;

    [Header("Предупреждающие звуки")]
    [SerializeField] private GameObject bankAngle;
    [SerializeField] private GameObject pullUp;
    [SerializeField] private GameObject checkSpeed;

    public void UpdateSounds(float rpm, float height)
    {
        if (rpm == 0 && height == 0)
        {
            engine.volume = 0;

            windGround.enabled = false;
            windAir.enabled = false;

            return;
        }

        float rpmAffect = rpm / 5000f;

        engine.volume = 0.3f + rpmAffect / 3 * 2;
        engine.pitch = 0.2f + rpmAffect;

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

    public void BankAngle(bool active)
    {
        bankAngle.SetActive(active);
    }

    public void PullUp(bool active)
    {
        pullUp.SetActive(active);
    }

    public void CheckSpeed(bool active)
    {
        checkSpeed.SetActive(active);
    }

    public void PlayWaterSplash()
    {
        waterSplash.Play();
    }

    public void PlayCollectedSound()
    {
        collectedSound.Play();
    }

    public void PlayExplosion(float magnitude)
    {
        explosion.volume = magnitude / 40f;
        explosion.pitch = 0.6f + magnitude / 80f;
        explosion.Play();
    }
}
