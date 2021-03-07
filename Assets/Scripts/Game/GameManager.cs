using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject competition;
    [SerializeField] private GameObject freeFlight;
    [SerializeField] private GameObject tutorial;
    private int gamemode;
    private Gamemode gm;

    private void Start()
    {
        gamemode = PlayerPrefs.GetInt("Gamemode");

        switch (gamemode)
        {
            case 0: // ������������
                competition.SetActive(true);
                break;
            case 1: // ��������� �����
                freeFlight.SetActive(true);
                break;
            case 2: // ��������
                tutorial.SetActive(true);
                break;
        }

        gm = FindObjectOfType<Gamemode>();
        if (gm != null)
        {
            gm.StartGame();
        }
    }
}
