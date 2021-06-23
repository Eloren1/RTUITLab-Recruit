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

    /* � ������� NextStage() � CheckForTaskCompletion() ����� ����� ��������. 
     * � ������ ���� ���� ������� �������, � ���� ���������� ��������� ����� � 
     * �������, ������� ����� ���������. �� ������ ������� � ������ �����, ������� 
     * ��� 3 ������� ����� ���� �� �������� � ���. */

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

    // ������ canCheck = true ������ ����� ����� �������,
    // ��� ��� ������� � ������ ��������� ����� ������������ ��������
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
                        LostGame("�� ���������� �� ������ ����� 3000 �����");
                    }
                    if (planeController.SpeedInKnots < 50f)
                    {
                        LostGame("�������� �������� ���������� ���� 50 �����");
                    }
                    break;
                case 1:
                    if (planeController.SpeedInKnots < 40f &&
                        planePrefab.transform.position.y * 3.28084f > 100f)
                    {
                        LostGame("�������� �������� ���������� ���� 40 �����");
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
                        text = "��� ��������� ���������� ������!\n������� ���� ����������.�";
                        break;
                    case 1:
                        StartCoroutine(ShowTask());
                        text = "��������� [W] � [S] ��� ��������� ���� �������";
                        break;
                    case 2:
                        StartCoroutine(ShowInstructorText());
                        text = "����������. �� �������� ������ �������.\n����� �� ������ �������� ��� ���������.�";
                        break;
                    case 3:
                        StartCoroutine(ShowInstructorText());
                        text = "��� �� ������ 5000 �����.\n��������� �� ������ 6000 �����.�";
                        break;
                    case 4:
                        StartCoroutine(ShowTask());
                        text = "����������� �� ������ 6000 �����";
                        break;
                    case 5:
                        StartCoroutine(ShowInstructorText());
                        text = "�� ������ ��� �����\n�������� ����������� ������.�";
                        break;
                    case 6:
                        StartCoroutine(ShowTask());
                        text = "��������� [A] � [D] ��� ��������� ���� �����";
                        break;
                    case 7:
                        StartCoroutine(ShowInstructorText());
                        text = "������� ����� ������ ���� �������,\n����� ������ ������������.�";
                        break;
                    case 8:
                        StartCoroutine(ShowTask());
                        text = "��������� [W] � [S] ��� ��������� ���� �������";
                        break;
                    case 9:
                        StartCoroutine(ShowInstructorText());
                        text = "�������� ������� ����� ��������� � ���� ���������� �� ����.�";
                        break;
                    case 10:
                        StartCoroutine(ShowInstructorText());
                        text = "������ ������ ������������,\n�� �������� ��� �������� �������. �� ��������.�";
                        break;
                    case 11:
                        StartCoroutine(ShowTask());
                        text = "���������� ������� �� 180 ��������";
                        break;
                    case 12:
                        StartCoroutine(ShowInstructorText());
                        text = "������������ ����� �� ���������.\n�� �������� �������� ���� 80 �����.�";
                        break;
                    case 13:
                        StartCoroutine(ShowInstructorText());
                        text = "���� ��������� �������� �������� � ���������\n��� �������� ������� ����.�";
                        break;
                    case 14:
                        StartCoroutine(ShowTask());
                        text = "��������� [R] � [F] ��� ����������� ��������";
                        break;
                    case 15:
                        StartCoroutine(ShowInstructorText());
                        text = "��������. ��� ��������� �������\n������� ������� ���� ����.�";
                        break;
                    case 16:
                        StartCoroutine(ShowTask());
                        text = "����������� �� ������ 9000 �����";
                        break;
                    case 17:
                        StartCoroutine(ShowInstructorText());
                        text = "��������.\n�� ������������ �� ����.�";
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
                        text = "�� ������� �� �������� ���� ������ �� ���������.�";
                        break;
                    case 1:
                        StartCoroutine(ShowInstructorText());
                        text = "��������� ���� �������������� ��������� ����.\n��� ����� ��� ��� ������ � �������.�";
                        break;
                    case 2:
                        StartCoroutine(ShowInstructorText());
                        text = "���� ������, ����� �������, ��� ����� ��������� �������� ����������.�";
                        break;
                    case 3:
                        StartCoroutine(ShowInstructorText());
                        text = "�� �����, ����� �� ���������� ���������� ������,\n�� ������ ������ ��������.�";
                        break;
                    case 4:
                        StartCoroutine(ShowTask());
                        text = "��������� [T] � [G] ��� ����������� ���������";
                        break;
                    case 5:
                        StartCoroutine(ShowInstructorText());
                        text = "������� ����� �������� ������� ���������.\n� �� ����� ������ �� ������.�";
                        break;
                    case 6:
                        StartCoroutine(ShowTask());
                        text = "��������� [R] � [F] ��� ����������� ��������";
                        break;
                    case 7:
                        StartCoroutine(ShowInstructorText());
                        text = "���� ���������� �������� 80%\n�� ���� �������� ���������.�";
                        break;
                    case 8:
                        StartCoroutine(ShowTask());
                        text = "����������� �� ������ 50 �����";
                        break;
                    case 9:
                        StartCoroutine(ShowInstructorText());
                        text = "������� ��� ���������� ������ �����\n��� ���������� ������������� � ��������.�";
                        break;
                    case 10:
                        StartCoroutine(ShowTask());
                        text = "��������� [C] ��� �������� ��� �������� �����";
                        break;
                    case 11:
                        StartCoroutine(ShowTask());
                        text = "����������� �� ������ 500 �����";
                        break;
                    case 12:
                        StartCoroutine(ShowInstructorText());
                        text = "��� �����? �� ������ ������ ������� ��������.�";
                        break;
                    case 13:
                        StartCoroutine(ShowTask());
                        text = "��������� [T] ��� ������� ���������";
                        break;
                    case 14:
                        StartCoroutine(ShowInstructorText());
                        text = "���� �������. ���������� ����� ������.�";
                        break;
                    case 15:
                        StartCoroutine(ShowInstructorText());
                        text = "������ �� ��������� � ��������� ���������,\n�� �������� �������� ������ 90 �����.�";
                        break;
                    case 16:
                        StartCoroutine(ShowTask());
                        text = "����������� �� ������ 4000 �����";
                        break;
                    case 17:
                        StartCoroutine(ShowInstructorText());
                        text = "������� ���������. ���������� �������\n� � ������ � ���� ����������.�";
                        break;
                    case 18:
                        StartCoroutine(ShowTask());
                        text = "���������� �������, ����� �� ����� �����";
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
                        text = "��� ��� ���������.\n������ �� ������ � ������ ����.�";
                        break;
                    case 1:
                        StartCoroutine(ShowInstructorText());
                        text = "������ �������� � ������ 80 �����.�";
                        break;
                    case 2:
                        StartCoroutine(ShowInstructorText());
                        text = "���������� ������ �������\n� ����������� �� �������� ���������.�";
                        break;
                    case 3:
                        StartCoroutine(ShowTask());
                        text = "��������� [R] � [F] ��� ����������� ��������";
                        break;
                    case 4:
                        StartCoroutine(ShowTask());
                        text = "��������� [T] � [G] ��� ����������� ���������";
                        break;
                    case 5:
                        StartCoroutine(ShowInstructorText());
                        text = "������ ������ ��� ���������� � �� ��������.�";
                        break;
                    case 6:
                        StartCoroutine(ShowInstructorText());
                        text = "������ �� ��������� ������ ��������.\n�� ������ ��������, �� ��������� ���������.�";
                        break;
                    case 7:
                        StartCoroutine(ShowTask());
                        text = "��������� [W] � [S] ��� ��������� ���� �������";
                        break;
                    case 8:
                        StartCoroutine(ShowInstructorText());
                        text = "������� �������� � ���������\n���������� ��������� ��������.�";
                        break;
                    case 9:
                        StartCoroutine(ShowTask());
                        text = "������� �������� ����� 70 �����";
                        break;
                    case 10:
                        StartCoroutine(ShowInstructorText());
                        text = "��� ����� ��� ��������� �����.�";
                        break;
                    case 11:
                        StartCoroutine(ShowTask());
                        text = "���������� �� ������ 300 �����";
                        break;
                    case 12:
                        StartCoroutine(ShowInstructorText());
                        text = "��������.�";
                        break;
                    case 13:
                        StartCoroutine(ShowTask());
                        text = "���������� �� ������ 20 �����";
                        break;
                    case 14:
                        StartCoroutine(ShowTask());
                        text = "��������� [B] ��� ���������� �������� �����";
                        break;
                    case 15:
                        StartCoroutine(ShowTask());
                        text = "��������� ���������� �������";
                        break;
                    case 16:
                        StartCoroutine(ShowInstructorText());
                        text = "�������� ���������.�";
                        break;
                    case 17:
                        StartCoroutine(ShowInstructorText());
                        text = "�������, � ���� ��� � �� �������,\n��� ������������ ���� �����������.�";
                        break;
                    case 18:
                        StartCoroutine(ShowTask());
                        text = "��������� [Q] � [E] ��� �������� ���� �����������";
                        break;
                    case 19:
                        StartCoroutine(ShowInstructorText());
                        text = "���������� ��� ��� ������� �� �����.�";
                        break;
                    case 20:
                        StartCoroutine(ShowInstructorText());
                        text = "������ �� ���������� ������������ ��� � �������.\n�� ������ �������� ����������.�";
                        break;
                    case 21:
                        StartCoroutine(ShowInstructorText());
                        text = "��� ��� �.\n�� ����������� ������� ����.�";
                        break;
                    case 22: 
                        StartCoroutine(ShowInstructorText());
                        text = "������ ������ ������ ��������������, ������� �� �����.�";
                         break;
                    case 23:
                        StartCoroutine(ShowInstructorText());
                        text = "������, ��� ��� ������� ����� ����������� �������\n�� ��������� ���������� �� ������.�";
                        break;  
                    case 24:
                        StartCoroutine(ShowInstructorText());
                        text = "�� ��� ����� ������ ������\n���������� � ����� �������.�";
                        break;
                    case 25:
                        StartCoroutine(ShowInstructorText());
                        text = "�������� �� �������, ���� � ���� ����!�";
                        break;
                    case 26:
                        StartCoroutine(ShowInstructorText());
                        text = "��� �������.�";
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

        gameUI.ShowEndingScreen((level + 1).ToString() + " ������� �������� �������", "", true);
    }
}