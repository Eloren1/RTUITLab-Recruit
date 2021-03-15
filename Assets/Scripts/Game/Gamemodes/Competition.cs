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

        Vector3 startingVelocity = Vector3.zero;
        float startingThrust = 0;

        switch (level)
        {
            case 0:
                startingVelocity = new Vector3(-20f, 3f, -25f);
                startingThrust = 0.7f;
                break;
            case 1:
                startingVelocity = new Vector3(-50f, -10f, -13f);
                startingThrust = 0.7f;
                break;
            case 2:
                startingVelocity = new Vector3(-50f, -10f, -13f);
                startingThrust = 0.7f;
                break;
        }

        SpawnPlane(planePrefab, spawns[level], startingVelocity, startingThrust);

        // Âêëþ÷àåì îáúåêò ñî âñåìè êîëüöàìè
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
        return ($"ÏÐÎËÅÒÈÒÅ ×ÅÐÅÇ ÊÎËÜÖÀ. ÎÑÒÀËÎÑÜ ÊÎËÅÖ: {circleCount}");
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

        FindObjectOfType<PlaneController>().StopGame();

        int currentRecord = PlayerPrefs.GetInt("Competition " + level);

        string subText = $"ÂÐÅÌß: { GetTime(seconds) }\n" +
                         $"ÐÅÊÎÐÄ: { GetTime(currentRecord) }";

        if ((currentRecord < 1) || (seconds > 1 && seconds < currentRecord))
        {
            PlayerPrefs.SetInt("Competition " + level, seconds);
        }

        gameUI.ShowEndingScreen((level + 1).ToString() + " ÓÐÎÂÅÍÜ ÑÎÐÅÂÍÎÂÀÍÈÉ ÏÐÎÉÄÅÍ", subText, false);
    }
}
