using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    [SerializeField] private Material[] skyboxes;
    [SerializeField] private Light[] directionalLights;

    private void Awake()
    {
        int weather = PlayerPrefs.GetInt("Weather");

        RenderSettings.skybox = skyboxes[weather];

        foreach(var light in directionalLights)
        {
            light.gameObject.SetActive(false);
        }
        directionalLights[weather].gameObject.SetActive(true);

        RenderSettings.fog = true;

        switch (weather)
        {
            case 0: // 30% облаков
                RenderSettings.fogEndDistance = 10000;
                RenderSettings.fogColor = new Color32(177, 212, 244, 255);
                break;
            case 1: // Туман, тучи
                RenderSettings.fogEndDistance = 1000;
                RenderSettings.fogColor = new Color32(37, 37, 37, 255);
                break;
            case 2: // Вечерний закат
                RenderSettings.fogEndDistance = 5000;
                RenderSettings.fogColor = new Color32(55, 70, 70, 255);
                break;
        }

        DynamicGI.UpdateEnvironment();
    }
}
