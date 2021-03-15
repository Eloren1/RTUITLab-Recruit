using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControllerMainMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] tabs;
    [SerializeField] private GameObject allTabs;

    [SerializeField] private GameObject mainMenu;

    [HideInInspector] public Vector2 OriginalScreenResolution;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Guides"))
        {
            AssignStartValues();
        }

        OriginalScreenResolution.x = Screen.width;
        OriginalScreenResolution.y = Screen.height;

        ChangeGraphics();
    }

    private void AssignStartValues()
    {
        PlayerPrefs.SetInt("Guides", 1); // Подсказки при посадке включены
        PlayerPrefs.SetInt("Graphics", 2); // Средний уровень графики
    }

    public void ChangeGraphics()
    {
        int graphics = PlayerPrefs.GetInt("Graphics");
        QualitySettings.SetQualityLevel(graphics, true);
    }

    private void Start()
    {
        Time.timeScale = 1;

        ReturnToMainMenu();
    }

    public void OpenTab(int id)
    {
        // Выключаем вкладку главного меню
        mainMenu.SetActive(false);

        allTabs.SetActive(true);
        // Включаем нужную вкладку
        tabs[id].SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        // Выключаем все вкладки
        foreach (var tab in tabs)
            tab.SetActive(false);
        allTabs.SetActive(false);

        // Включаем вкладку главного меню
        mainMenu.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
