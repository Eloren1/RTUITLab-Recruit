using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : Gamemode
{
    private int level;
    [SerializeField] private GameObject planePrefab;
    private PlaneController planeController;
    private Engine engine;
    [SerializeField] private Transform[] spawns;

    private int stage = -1;
    private bool waitForAction = false;
    private string text;

    private bool gameActive = false;
    private bool canCheck = false;
    private GameUIController gameUI;

    private void Awake()
    {
        gameUI = FindObjectOfType<GameUIController>();
    }

    /* В методах NextStage() и CheckForTaskCompletion() очень много хардкода. 
     * В идеале надо было сделать таблицу, и туда записывать выводимый текст и 
     * задание, которое нужно выполнить. Но проект делался в сжатые сроки, поэтому 
     * для 3 уровней можно было бы обойтись и так. */

    public override void StartGame()
    {
        level = PlayerPrefs.GetInt("Level");

        planeController = planePrefab.GetComponent<PlaneController>();
        engine = planePrefab.GetComponent<Engine>();

        Vector3 startingVelocity = Vector3.zero;
        float startingThrust = 0;

        switch (level)
        {
            case 0:
                startingVelocity = new Vector3(70f, -2f, 70f);
                startingThrust = 0.8f;
                break;
            case 1:
                startingVelocity = Vector3.zero;
                startingThrust = 0.02f;
                break;
            case 2:
                startingVelocity = new Vector3(0f, 3f, 35f);
                startingThrust = 0.7f;
                break;
        }

        SpawnPlane(planePrefab, spawns[level], startingVelocity, startingThrust);

        gameActive = true;
        StartCoroutine(SetCanCheck());

        NextStage();
    }

    // Делаем canCheck = true спустя время после запуска,
    // Так как самолет в момент появления имеет неправильную скорость
    private IEnumerator SetCanCheck()
    {
        yield return new WaitForSeconds(0.2f);
        canCheck = true;
    }

    private void Update()
    {
        if (gameUI != null && canCheck)
        {
            switch (level)
            {
                case 0:
                    if (planePrefab.transform.position.y * 3.28084f < 3000f)
                    {
                        LostGame("ВЫ СПУСТИЛИСЬ НА ВЫСОТУ МЕНЕЕ 3000 ФУТОВ");
                    }
                    if (planeController.SpeedInKnots < 50f)
                    {
                        LostGame("СКОРОСТЬ САМОЛЕТА ОПУСТИЛАСЬ НИЖЕ 50 УЗЛОВ");
                    }
                    break;
                case 1:
                    if (planeController.SpeedInKnots < 40f &&
                        planePrefab.transform.position.y * 3.28084f > 100f)
                    {
                        LostGame("СКОРОСТЬ САМОЛЕТА ОПУСТИЛАСЬ НИЖЕ 40 УЗЛОВ");
                    }
                    break;
                case 2: goto case 1;
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

                case 2:
                    switch (stage)
                    {
                        case 3:
                            if (Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.F))
                            {
                                StartCoroutine(TaskCompleted());
                            }
                            break;
                        case 4:
                            if (Input.GetKey(KeyCode.G) || Input.GetKey(KeyCode.T))
                            {
                                StartCoroutine(TaskCompleted());
                            }
                            break;
                        case 7:
                            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                            {
                                StartCoroutine(TaskCompleted());
                            }
                            break;
                        case 9:
                            if (planeController.SpeedInKnots < 75 && planeController.SpeedInKnots > 65)
                            {
                                StartCoroutine(TaskCompleted());
                            }
                            break;
                        case 11:
                            if (planePrefab.transform.position.y * 3.28084f < 300)
                            {
                                StartCoroutine(TaskCompleted());
                            }
                            break;
                        case 13:
                            if (planePrefab.transform.position.y * 3.28084f < 20)
                            {
                                StartCoroutine(TaskCompleted());
                            }
                            break;
                        case 14:
                            if (Input.GetKey(KeyCode.B))
                            {
                                StartCoroutine(TaskCompleted());
                            }
                            break;
                        case 15:
                            if (Mathf.Abs(planeController.SpeedInKnots) < 2f && engine.rpm < 150f)
                            {
                                StartCoroutine(TaskCompleted());
                            }
                            break;
                        case 18:
                            if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E))
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
                        text = "«Внимательно следи за скоростью.\nНе допускай скорости ниже 80 узлов.»";
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
                        text = "«Для начала, перед взлетом, нам нужно выпустить закрылки наполовину.»";
                        break;
                    case 3:
                        StartCoroutine(ShowInstructorText());
                        text = "«А потом, когда мы поднимемся достаточно высоко,\nты должен убрать закрылки.»";
                        break;
                    case 4:
                        StartCoroutine(ShowTask());
                        text = "ЗАЖИМАЙТЕ [T] и [G] ДЛЯ РЕГУЛИРОВКИ ЗАКРЫЛКОВ";
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
                        text = "«Нам достаточно примерно 80%\nот всей мощности двигателя.»";
                        break;
                    case 8:
                        StartCoroutine(ShowTask());
                        text = "ПОДНИМИТЕСЬ НА ВЫСОТУ 50 ФУТОВ";
                        break;
                    case 9:
                        StartCoroutine(ShowInstructorText());
                        text = "«Теперь нам необходимо убрать шасси\nдля уменьшения сопротивления с воздухом.»";
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

            case 2:
                stage++;
                switch (stage)
                {
                    case 0:
                        StartCoroutine(ShowInstructorText());
                        text = "«Мы уже подлетаем.\nНичего не трогай и слушай меня.»";
                        break;
                    case 1:
                        StartCoroutine(ShowInstructorText());
                        text = "«Держи скорость в районе 80 узлов.»";
                        break;
                    case 2:
                        StartCoroutine(ShowInstructorText());
                        text = "«Понемногу снижай обороты\nи компенсируй их выпуском закрылков.»";
                        break;
                    case 3:
                        StartCoroutine(ShowTask());
                        text = "ЗАЖИМАЙТЕ [R] И [F] ДЛЯ РЕГУЛИРОВКИ ОБОРОТОВ";
                        break;
                    case 4:
                        StartCoroutine(ShowTask());
                        text = "ЗАЖИМАЙТЕ [T] и [G] ДЛЯ РЕГУЛИРОВКИ ЗАКРЫЛКОВ";
                        break;
                    case 5:
                        StartCoroutine(ShowInstructorText());
                        text = "«Нужно делать все постепенно и не торопясь.»";
                        break;
                    case 6:
                        StartCoroutine(ShowInstructorText());
                        text = "«Следи за скоростью нашего снижения.\nМы должны медленно, но постоянно снижаться.»";
                        break;
                    case 7:
                        StartCoroutine(ShowTask());
                        text = "ЗАЖИМАЙТЕ [W] И [S] ДЛЯ ИЗМЕНЕНИЯ УГЛА ТАНГАЖА";
                        break;
                    case 8:
                        StartCoroutine(ShowInstructorText());
                        text = "«Снижай скорость и продолжай\nпостепенно выпускать закрылки.»";
                        break;
                    case 9:
                        StartCoroutine(ShowTask());
                        text = "ДЕРЖИТЕ СКОРОСТЬ ОКОЛО 70 УЗЛОВ";
                        break;
                    case 10:
                        StartCoroutine(ShowInstructorText());
                        text = "«Мы можем уже выпускать шасси.»";
                        break;
                    case 11:
                        StartCoroutine(ShowTask());
                        text = "ОПУСТИТЕСЬ НА ВЫСОТУ 300 ФУТОВ";
                        break;
                    case 12:
                        StartCoroutine(ShowInstructorText());
                        text = "«Садимся.»";
                        break;
                    case 13:
                        StartCoroutine(ShowTask());
                        text = "ОПУСТИТЕСЬ НА ВЫСОТУ 20 ФУТОВ";
                        break;
                    case 14:
                        StartCoroutine(ShowTask());
                        text = "ЗАЖИМАЙТЕ [B] ДЛЯ ТОРМОЖЕНИЯ КОЛЕСАМИ ШАССИ";
                        break;
                    case 15:
                        StartCoroutine(ShowTask());
                        text = "ПОЛНОСТЬЮ ОСТАНОВИТЕ САМОЛЕТ";
                        break;
                    case 16:
                        StartCoroutine(ShowInstructorText());
                        text = "«Отлично сработано.»";
                        break;
                    case 17:
                        StartCoroutine(ShowInstructorText());
                        text = "«Кстати, я тебе так и не показал,\nкак использовать руль направления.»";
                        break;
                    case 18:
                        StartCoroutine(ShowTask());
                        text = "ЗАЖИМАЙТЕ [Q] И [E] ДЛЯ ПОВОРОТА РУЛЯ НАПРАВЛЕНИЯ";
                        break;
                    case 19:
                        StartCoroutine(ShowInstructorText());
                        text = "«Используй его для руления на земле.»";
                        break;
                    case 20:
                        StartCoroutine(ShowInstructorText());
                        text = "«Очень не рекомендую использовать его в воздухе.\nТы можешь потерять управление.»";
                        break;
                    case 21:
                        StartCoroutine(ShowInstructorText());
                        text = "«Ну что ж.\nТы великолепно показал себя.»";
                        break;
                    case 22: 
                        StartCoroutine(ShowInstructorText());
                        text = "«Когда будешь летать самостоятельно, никогда не спеши.»";
                         break;
                    case 23:
                        StartCoroutine(ShowInstructorText());
                        text = "«Помни, что для посадки нужно выравнивать самолет\nза несколько километров до полосы.»";
                        break;  
                    case 24:
                        StartCoroutine(ShowInstructorText());
                        text = "«И что пилот всегда должен\nстремиться к новым знаниям.»";
                        break;
                    case 25:
                        StartCoroutine(ShowInstructorText());
                        text = "«Никогда не забывай, чему я тебя учил!»";
                        break;
                    case 26:
                        StartCoroutine(ShowInstructorText());
                        text = "«До встречи.»";
                        break;
                    case 27:
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
            planeController.StopGame();

            gameActive = false;

            gameUI.ShowEndingScreen(reason, "", false);
        }
    }

    public override void CompletedGame()
    {
        planeController.StopGame();

        gameUI.ShowEndingScreen((level + 1).ToString() + " УРОВЕНЬ ОБУЧЕНИЯ ПРОЙДЕН", "", true);
    }
}