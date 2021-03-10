using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : Gamemode
{
    private int level;
    [SerializeField] private GameObject planePrefab;
    [SerializeField] private Transform[] spawns;

    private int stage = -1;
    private bool waitForAction = false;
    private string text;

    private bool gameActive = true;
    private GameUIController gameUI;

    private void Awake()
    {
        gameUI = FindObjectOfType<GameUIController>();
    }

    public override void StartGame()
    {
        level = PlayerPrefs.GetInt("Level");

        // Перемещаем самолет в нужную точку
        planePrefab.transform.position = spawns[level].transform.position;
        planePrefab.transform.rotation = spawns[level].transform.rotation;

        switch (level)
        {
            case 0:
                planePrefab.GetComponent<PlaneController>().SetStartValues(new Vector3(74.6f, -0.9f, 72.3f), 0.7f);
                break;
            case 1:
                planePrefab.GetComponent<PlaneController>().SetStartValues(Vector3.zero, 0);
                break;
            case 2:
                planePrefab.GetComponent<PlaneController>().SetStartValues(Vector3.zero, 0);
                break;
        }

        NextStage();
    }

    private void Update()
    {
        if (gameUI != null)
        {
            if (planePrefab.transform.position.y * 3.28084f < 4000f)
            {
                LostGame();
            }

            CheckForTaskCompletion();
        }
    }

    private void CheckForTaskCompletion()
    {
        if (waitForAction)
        {
            switch (level)
            {
                case 0:
                    switch (stage)
                    {
                        case 1:
                            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                            {
                                StartCoroutine(TaskCompleted());
                            }
                            break;
                        case 4:
                            if (planePrefab.transform.position.y * 3.28084f > 6000)
                            {
                                StartCoroutine(TaskCompleted());
                            }
                            break;
                        case 6:
                            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                            {
                                StartCoroutine(TaskCompleted());
                            }
                            break;
                        case 8:
                            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                            {
                                StartCoroutine(TaskCompleted());
                            }
                            break;
                        case 11:
                            if (planePrefab.transform.eulerAngles.y > 170 || planePrefab.transform.eulerAngles.y < -170)
                            {
                                StartCoroutine(TaskCompleted());
                            }
                            break;
                        case 14:
                            if (Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.F))
                            {
                                StartCoroutine(TaskCompleted());
                            }
                            break;
                        case 16:
                            if (planePrefab.transform.position.y * 3.28084f > 9000)
                            {
                                StartCoroutine(TaskCompleted());
                            }
                            break;
                    }
                    break;
            }
        }
    }

    private void NextStage()
    {
        switch (level)
        {
            case 0:
                stage++;
                switch (stage)
                {
                    case 0:
                        StartCoroutine(ShowInstructorText());
                        text = "«Мы поднялись достаточно высоко!\nПередаю тебе управление.»";
                        break;
                    case 1:
                        StartCoroutine(ShowTask());
                        text = "ИСПОЛЬЗУЙТЕ [W] И [S] ДЛЯ ИЗМЕНЕНИЯ УГЛА ТАНГАЖА";
                        break;
                    case 2:
                        StartCoroutine(ShowInstructorText());
                        text = "«Спокойнее. Не накреняй сильно самолет.\nСкоро ты будешь понимать его поведение.»";
                        break;
                    case 3:
                        StartCoroutine(ShowInstructorText());
                        text = "«Мы на высоте 5000 футов.\nПоднимись на высоту в 6000 футов.»";
                        break;
                    case 4:
                        StartCoroutine(ShowTask());
                        text = "ПОДНИМИТЕСЬ НА ВЫСОТУ В 6000 ФУТОВ";
                        break;
                    case 5:
                        StartCoroutine(ShowInstructorText());
                        text = "«А теперь нам нужно\nизменить направление полета.»";
                        break;
                    case 6:
                        StartCoroutine(ShowTask());
                        text = "ИСПОЛЬЗУЙТЕ [A] И [D] ДЛЯ ИЗМЕНЕНИЯ УГЛА КРЕНА";
                        break;
                    case 7:
                        StartCoroutine(ShowInstructorText());
                        text = "«Теперь снова измени угол тангажа,\nчтобы начать поворачивать.»";
                        break;
                    case 8:
                        StartCoroutine(ShowTask());
                        text = "ИСПОЛЬЗУЙТЕ [W] И [S] ДЛЯ ИЗМЕНЕНИЯ УГЛА ТАНГАЖА";
                        break;
                    case 9:
                        StartCoroutine(ShowInstructorText());
                        text = "«Давай изменим направление полета на 180 градусов.»";
                        break;
                    case 10:
                        StartCoroutine(ShowInstructorText());
                        text = "«Накрени самолет посильнее и изменяй тангаж.»";
                        break;
                    case 11:
                        StartCoroutine(ShowTask());
                        text = "РАЗВЕРНИТЕ САМОЛЕТ НА 180 ГРАДУСОВ";
                        break;
                    case 12:
                        StartCoroutine(ShowInstructorText());
                        text = "«Внимательно следи за скоростью.\nНе допускай скорости ниже 90 узлов.»";
                        break;
                    case 13:
                        StartCoroutine(ShowInstructorText());
                        text = "«Если понадобится, добавляй оборотов в двигателе.»";
                        break;
                    case 14:
                        StartCoroutine(ShowTask());
                        text = "ИСПОЛЬЗУЙТЕ [R] И [F] ДЛЯ ИЗМЕНЕНИЯ ОБОРОТОВ";
                        break;
                    case 15:
                        StartCoroutine(ShowInstructorText());
                        text = "«Отлично. Для окончания занятия\nподними самолет чуть выше.»";
                        break;
                    case 16:
                        StartCoroutine(ShowTask());
                        text = "ПОДНИМИТЕСЬ НА ВЫСОТУ В 9000 ФУТОВ";
                        break;
                    case 17:
                        StartCoroutine(ShowInstructorText());
                        text = "«Молодец.\nМы возвращаемся на базу.»";
                        break;
                    case 18:
                        CompletedGame();
                        break;
                }
                break;
        }
    }

    private IEnumerator ShowInstructorText()
    {
        yield return new WaitForSeconds(0.5f);

        gameUI.SetSubTask(text);

        yield return new WaitForSeconds(5);

        gameUI.SetSubTask("");

        yield return new WaitForSeconds(0.5f);

        NextStage();
    }

    private IEnumerator ShowTask()
    {
        yield return new WaitForSeconds(0.5f);

        gameUI.SetTask(text);

        yield return new WaitForSeconds(2);

        waitForAction = true;
    }

    private IEnumerator TaskCompleted()
    {
        waitForAction = false;

        gameUI.SetTask("");

        yield return new WaitForSeconds(0.5f);

        NextStage();
    }

    private void LostGame()
    {
        if (gameActive)
        {
            gameActive = false;

            // Вы спустились на высоту менее 4000 футов
        }
    }

    public override void CompletedGame()
    {
        throw new System.NotImplementedException();
    }
}