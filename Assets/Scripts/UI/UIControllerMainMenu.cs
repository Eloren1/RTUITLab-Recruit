using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControllerMainMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] tabs;
    [SerializeField] private GameObject allTabs;

    [SerializeField] private GameObject mainMenu;

    private void Start()
    {
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
