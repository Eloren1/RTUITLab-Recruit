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

        switch (weather)
        {
            case 0: // 30% �������
                RenderSettings.fog = false;
                break;
            case 1: // �����, �������
                RenderSettings.fog = true;
                break;
            case 2: // �������� �����
                RenderSettings.fog = false;
                break;
        }

        DynamicGI.UpdateEnvironment();
    }
}
