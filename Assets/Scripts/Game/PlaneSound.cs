using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneSound : MonoBehaviour
{
    [Header("Одиночные звуки")]
    [SerializeField] private AudioSource waterSplash;
    [SerializeField] private AudioSource collectedSound;

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
}
