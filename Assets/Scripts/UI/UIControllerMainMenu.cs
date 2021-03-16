using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControllerMainMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] tabs;
    [SerializeField] private GameObject allTabs;

    [SerializeField] private GameObject mainMenu;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Guides"))
        {
            AssignStartValues();
        }

        ChangeGraphics();
    }

    private void AssignStartValues()
    {
        PlayerPrefs.SetInt("Guides", 1); // ��������� ��� ������� ��������
        PlayerPrefs.SetInt("Graphics", 2); // ������� ������� �������
    }

    public void ChangeGraphics()
    {
        int graphics = PlayerPrefs.GetInt("Graphics");
        QualitySettings.SetQualityLevel(graphics, true);
    }

    private void Start()
    {
        Time.timeScale = 1;

        ReturnToMainMenuSilent();
    }

    public void OpenTab(int id)
    {
        UISoundManager.Instance.PlayClickSound();

        // ��������� ������� �������� ����
        mainMenu.SetActive(false);

        allTabs.SetActive(true);
        // �������� ������ �������
        tabs[id].SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        if (UISoundManager.Instance != null)
            UISoundManager.Instance.PlayClickSound();

        // ��������� ��� �������
        foreach (var tab in tabs)
            tab.SetActive(false);
        allTabs.SetActive(false);

        // �������� ������� �������� ����
        mainMenu.SetActive(true);
    }

    public void ReturnToMainMenuSilent()
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
