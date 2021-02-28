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
        // ��������� ������� �������� ����
        mainMenu.SetActive(false);

        allTabs.SetActive(true);
        // �������� ������ �������
        tabs[id].SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        // ��������� ��� �������
        foreach (var tab in tabs)
            tab.SetActive(false);
        allTabs.SetActive(false);

        // �������� ������� �������� ����
        mainMenu.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
