using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int gamemode;
    [SerializeField] private GameObject competition;
    [SerializeField] private GameObject freeFlight;
    [SerializeField] private GameObject tutorial;

    private Gamemode gm;

    private void Start()
    {
        gamemode = PlayerPrefs.GetInt("Gamemode");

        switch (gamemode)
        {
            case 0: // Соревнования
                competition.SetActive(true);
                break;
            case 1: // Свободный полет
                freeFlight.SetActive(true);
                break;
            case 2: // Обучение
                tutorial.SetActive(true);
                break;
        }

        gm = FindObjectOfType<Gamemode>();

        gm.StartGame();
    }
}
