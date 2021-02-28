using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Competition : Gamemode
{
    private int level;

    [SerializeField] private GameObject planePrefab;

    [SerializeField] private Transform[] spawns;
    [SerializeField] private GameObject[] levels;
    private int circleCount;
    private int seconds;

    private GameUIController gameUI;

    public void Awake()
    {
        gameUI = FindObjectOfType<GameUIController>();
    }

    public override void StartGame()
    {
        seconds = 0;
        level = PlayerPrefs.GetInt("Level");

        // Создаем самолет в нужной точке
        Instantiate(planePrefab, spawns[level].transform.position, spawns[level].transform.rotation);
        planePrefab.GetComponent<PlaneController>().SetStartValues(Vector3.zero, 0, 0); // TODO: Set real values!

        // Включаем объект со всеми кольцами
        levels[level].SetActive(true);

        circleCount = CountCircles();
        gameUI.SetTask(GetTask(circleCount));

        StopAllCoroutines();
        StartCoroutine(Timer());
    }

    private int CountCircles()
    {
        return FindObjectsOfType<Circle>().Length;
    }

    public void CircleCollected()
    {
        circleCount--;

        gameUI.SetTask(GetTask(circleCount));

        if (circleCount <= 0)
        {
            CompletedGame();
        }
    }

    private string GetTask(int circleCount)
    {
        return ($"ПРОЛЕТИТЕ ЧЕРЕЗ КОЛЬЦА. ОСТАЛОСЬ КОЛЕЦ: {circleCount}");
    }

    private string GetTime(int time)
    {
        int seconds = time % 60;
        int minutes = (time / 60) % 60;
        int hours = (time / 60) / 60;

        return ("ВРЕМЯ: " + ((seconds > 9) ? "" : "0") + seconds +
           ((minutes > 9) ? "" : "0") + minutes + ((hours > 9) ? "" : "0") + hours);
    }

    private IEnumerator Timer()
    {
        while (true) // TODO: Make statement
        {
            yield return new WaitForSeconds(1);
            seconds++;
            gameUI.SetTime(GetTime(seconds));
        }
    }

    public override void CompletedGame()
    {
        StopAllCoroutines();
        int currentRecord = PlayerPrefs.GetInt("Competition " + level);

        // TODO: Say about old record, new time and button to exit or rerun

        if (currentRecord > 1 && seconds > 1 && seconds < currentRecord)
        {
            PlayerPrefs.SetInt("Competition " + level, seconds);
        }
    }
}
