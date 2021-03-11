using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : Gamemode
{
    private int level;
    [SerializeField] private GameObject planePrefab;
    private PlaneController planeController;
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

        planeController = planePrefab.GetComponent<PlaneController>();

        switch (level)
        {
            case 0:
                planePrefab.GetComponent<PlaneController>().SetStartValues(new Vector3(74.6f, -0.9f, 72.3f), 0.7f);
                break;
            case 1:
                planePrefab.GetComponent<PlaneController>().SetStartValues(Vector3.zero, 0.05f);
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
            switch (level)
            {
                case 0:
                    if (planePrefab.transform.position.y * 3.28084f < 4000f)
                    {
                        LostGame("ВЫ СПУСТИЛИСЬ НА ВЫСОТУ МЕНЕЕ 4000 ФУТОВ");
                    }
                    if (planeController.Magnitude * 3.6f * 0.53996f < 50f)
                    {
                        LostGame("СКОРОСТЬ САМОЛЕТА ОПУСТИЛАСЬ НИЖЕ 50 УЗЛОВ");
                    }
                    break;
                case 1:
                    if (planeController.Magnitude * 3.6f * 0.53996f < 40f &&
                        planePrefab.transform.position.y * 3.28084f > 100f)
                    {
                        LostGame("СКОРОСТЬ САМОЛЕТА ОПУСТИЛАСЬ НИЖЕ 40 УЗЛОВ");
                    }
                    break;
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

                case 1:
                    switch (stage)
                    {
                        case 4:
                            if (Input.GetKey(KeyCode.G))
                            {
                                StartCoroutine(TaskCompleted());
                            }
                            break;
                        case 6:
                            if (Input.GetKey(KeyCode.R))
                            {
                                StartCoroutine(TaskCompleted());
                            }
                            break;
                        case 8:
                            if (planePrefab.transform.position.y * 3.28084f > 50)
                            {
                                StartCoroutine(TaskCompleted());
                            }
                            break;
                        case 10:
                            if (Input.GetKey(KeyCode.C))
                            {
                                StartCoroutine(TaskCompleted());
                            }
                            break;
                        case 11:
                            if (planePrefab.transform.position.y * 3.28084f > 500)
                            {
                                StartCoroutine(TaskCompleted());
                            }
                            break;
                        case 13:
                            if (Input.GetKey(KeyCode.T))
                            {
                                StartCoroutine(TaskCompleted());
                            }
                            break;
                        case 16:
                            if (planePrefab.transform.position.y * 3.28084f > 4000)
                            {
                                StartCoroutine(TaskCompleted());
                            }
                            break;
                        case 18:
                            float xAngle = Mathf.Abs(planePrefab.transform.eulerAngles.x > 180 ?
                                planePrefab.transform.eulerAngles.x - 360 : planePrefab.transform.eulerAngles.x);
                            float zAngle = Mathf.Abs(planePrefab.transform.eulerAngles.z > 180 ? 
                                planePrefab.transform.eulerAngles.z - 360 : planePrefab.transform.eulerAngles.z);

                            if (xAngle < 15 && zAngle < 15)
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
                        text = "ЗАЖИМАЙТЕ [W] И [S] ДЛЯ ИЗМЕНЕНИЯ УГЛА ТАНГАЖА";
                        break;
                    case 2:
                        StartCoroutine(ShowInstructorText());
                        text = "«Спокойнее. Не накреняй сильно самолет.\nСкоро ты будешь понимать его поведение.»";
                        break;
                    case 3:
                        StartCoroutine(ShowInstructorText());
                        text = "«Мы на высоте 5000 футов.\nПоднимись на высоту 6000 футов.»";
                        break;
                    case 4:
                        StartCoroutine(ShowTask());
                        text = "ПОДНИМИТЕСЬ НА ВЫСОТУ 6000 ФУТОВ";
                        break;
                    case 5:
                        StartCoroutine(ShowInstructorText());
                        text = "«А теперь нам нужно\nизменить направление полета.»";
                        break;
                    case 6:
                        StartCoroutine(ShowTask());
                        text = "ЗАЖИМАЙТЕ [A] И [D] ДЛЯ ИЗМЕНЕНИЯ УГЛА КРЕНА";
                        break;
                    case 7:
                        StartCoroutine(ShowInstructorText());
                        text = "«Теперь снова измени угол тангажа,\nчтобы начать поворачивать.»";
                        break;
                    case 8:
                        StartCoroutine(ShowTask());
                        text = "ЗАЖИМАЙТЕ [W] И [S] ДЛЯ ИЗМЕНЕНИЯ УГЛА ТАНГАЖА";
                        break;
                    case 9:
                        StartCoroutine(ShowInstructorText());
                        text = "«Накрени самолет набок посильнее и тяни управление на себя.»";
                        break;
                    case 10:
                        StartCoroutine(ShowInstructorText());
                        text = "«Когда будешь поворачивать,\nне поднимай нос самолета навверх. Не торопись.»";
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
                        text = "«Для ускорения добавляй оборотов в двигателе\nили наклоняй самолет вниз.»";
                        break;
                    case 14:
                        StartCoroutine(ShowTask());
                        text = "ЗАЖИМАЙТЕ [R] И [F] ДЛЯ РЕГУЛИРОВКИ ОБОРОТОВ";
                        break;
                    case 15:
                        StartCoroutine(ShowInstructorText());
                        text = "«Отлично. Для окончания занятия\nподними самолет чуть выше.»";
                        break;
                    case 16:
                        StartCoroutine(ShowTask());
                        text = "ПОДНИМИТЕСЬ НА ВЫСОТУ 9000 ФУТОВ";
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

            case 1:
                stage++;
                switch (stage)
                {
                    case 0:
                        StartCoroutine(ShowInstructorText());
                        text = "«А сегодня мы закрепим твои знания по закрылкам.»";
                        break;
                    case 1:
                        StartCoroutine(ShowInstructorText());
                        text = "«Закрылки дают дополнительную подъемную силу.\nОна нужна нам при взлете и посадке.»";
                        break;
                    case 2:
                        StartCoroutine(ShowInstructorText());
                        text = "«Для начала, перед взлетом, нам нужно выпустить закрылки.»";
                        break;
                    case 3:
                        StartCoroutine(ShowInstructorText());
                        text = "«А потом, когда мы поднимемся достаточно высоко,\nты должен убрать закрылки.»";
                        break;
                    case 4:
                        StartCoroutine(ShowTask());
                        text = "ЗАЖИМАЙТЕ [G] ДЛЯ ВЫПУСКА ЗАКРЫЛКОВ";
                        break;
                    case 5:
                        StartCoroutine(ShowInstructorText());
                        text = "«Теперь нужно повысить обороты двигателя.\nИ мы будем готовы ко взлету.»";
                        break;
                    case 6:
                        StartCoroutine(ShowTask());
                        text = "ЗАЖИМАЙТЕ [R] И [F] ДЛЯ РЕГУЛИРОВКИ ОБОРОТОВ";
                        break;
                    case 7:
                        StartCoroutine(ShowInstructorText());
                        text = "«Нам достаточно примерно 90%\nот всей мощности двигателя.»";
                        break;
                    case 8:
                        StartCoroutine(ShowTask());
                        text = "ПОДНИМИТЕСЬ НА ВЫСОТУ 50 ФУТОВ";
                        break;
                    case 9:
                        StartCoroutine(ShowInstructorText());
                        text = "«Теперь нам необходимо убрать шасси\nдля уменьшения сопротивления воздуха.»";
                        break;
                    case 10:
                        StartCoroutine(ShowTask());
                        text = "НАЖИМАЙТЕ [C] ДЛЯ ОТКРЫТИЯ ИЛИ ЗАКРЫТИЯ ШАССИ";
                        break;
                    case 11:
                        StartCoroutine(ShowTask());
                        text = "ПОДНИМИТЕСЬ НА ВЫСОТУ 500 ФУТОВ";
                        break;
                    case 12:
                        StartCoroutine(ShowInstructorText());
                        text = "«Не забыл? Ты всегда должен убирать закрылки.»";
                        break;
                    case 13:
                        StartCoroutine(ShowTask());
                        text = "ЗАЖИМАЙТЕ [T] ДЛЯ СКРЫТИЯ ЗАКРЫЛКОВ";
                        break;
                    case 14:
                        StartCoroutine(ShowInstructorText());
                        text = "«Все отлично. Продолжаем набор высоты.»";
                        break;
                    case 15:
                        StartCoroutine(ShowInstructorText());
                        text = "«Следи за скоростью и оборотами двигателя,\nне допускай скорости меньше 90 узлов.»";
                        break;
                    case 16:
                        StartCoroutine(ShowTask());
                        text = "ПОДНИМИТЕСЬ НА ВЫСОТУ 4000 ФУТОВ";
                        break;
                    case 17:
                        StartCoroutine(ShowInstructorText());
                        text = "«Хорошо сработано. Выравнивай самолет\nи я заберу у тебя управление.»";
                        break;
                    case 18:
                        StartCoroutine(ShowTask());
                        text = "ВЫРОВНЯЙТЕ САМОЛЕТ, ЧТОБЫ ОН ЛЕТЕЛ ПРЯМО";
                        break;
                    case 19:
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

        yield return new WaitForSeconds(6);

        gameUI.SetSubTask("");

        yield return new WaitForSeconds(0.5f);

        NextStage();
    }

    private IEnumerator ShowTask()
    {
        yield return new WaitForSeconds(0.5f);

        gameUI.SetTask(text);

        waitForAction = true;
    }

    private IEnumerator TaskCompleted()
    {
        waitForAction = false;

        yield return new WaitForSeconds(1f);

        gameUI.SetTask("");

        yield return new WaitForSeconds(0.5f);

        NextStage();
    }

    private void LostGame(string reason)
    {
        if (gameActive)
        {
            gameActive = false;

            Debug.LogError("Реализовать экран проигрыша! " + reason);
        }
    }

    public override void CompletedGame()
    {
        throw new System.NotImplementedException();
    }
}