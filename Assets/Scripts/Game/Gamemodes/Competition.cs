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

        // Ïåğåìåùàåì ñàìîëåò â íóæíóş òî÷êó
        planePrefab.transform.position = spawns[level].transform.position;
        planePrefab.transform.rotation = spawns[level].transform.rotation;

        switch (level)
        {
            case 0:
                planePrefab.GetComponent<PlaneController>().SetStartValues(new Vector3(-20f, 3f, -25f), 0.7f);
                break;
            case 1:
                planePrefab.GetComponent<PlaneController>().SetStartValues(Vector3.zero, 0);
                break;
            case 2:
                planePrefab.GetComponent<PlaneController>().SetStartValues(new Vector3(-20f, 3f, -25f), 0.7f);
                break;
        }

        // Âêëş÷àåì îáúåêò ñî âñåìè êîëüöàìè
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
        return ($"ÏĞÎËÅÒÈÒÅ ×ÅĞÅÇ ÊÎËÜÖÀ. ÎÑÒÀËÎÑÜ ÊÎËÅÖ: {circleCount}");
    }

    private string GetTime(int time)
    {
        if (time <= 0) { return "--:--:--"; }

        int seconds = time % 60;
        int minutes = (time / 60) % 60;
        int hours = (time / 60) / 60;

        return (((hours > 9) ? "" : "0") + hours + ":" +
           ((minutes > 9) ? "" : "0") + minutes + ":" +((seconds > 9) ? "" : "0") + seconds);
    }

    private IEnumerator Timer()
    {
        while (true)
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

        string subText = $"ÂĞÅÌß: { GetTime(seconds) }\n" +
                         $"ĞÅÊÎĞÄ: { GetTime(currentRecord) }";

        if (currentRecord > 1 && seconds > 1 && seconds < currentRecord)
        {
            PlayerPrefs.SetInt("Competition " + level, seconds);
        }

        gameUI.ShowEndingScreen((level + 1).ToString() + " ÓĞÎÂÅÍÜ ÑÎĞÅÂÍÎÂÀÍÈÉ ÏĞÎÉÄÅÍ", subText, false);
    }
}
