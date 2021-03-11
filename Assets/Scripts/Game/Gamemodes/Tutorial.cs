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

        // ���������� ������� � ������ �����
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
                        LostGame("�� ���������� �� ������ ����� 4000 �����");
                    }
                    if (planeController.Magnitude * 3.6f * 0.53996f < 50f)
                    {
                        LostGame("�������� �������� ���������� ���� 50 �����");
                    }
                    break;
                case 1:
                    if (planeController.Magnitude * 3.6f * 0.53996f < 40f &&
                        planePrefab.transform.position.y * 3.28084f > 100f)
                    {
                        LostGame("�������� �������� ���������� ���� 40 �����");
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
                        text = "������������ ����� �� ���������.\n�� �������� �������� ���� 90 �����.�";
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
                        text = "���� ������, ����� �������, ��� ����� ��������� ��������.�";
                        break;
                    case 3:
                        StartCoroutine(ShowInstructorText());
                        text = "�� �����, ����� �� ���������� ���������� ������,\n�� ������ ������ ��������.�";
                        break;
                    case 4:
                        StartCoroutine(ShowTask());
                        text = "��������� [G] ��� ������� ���������";
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
                        text = "���� ���������� �������� 90%\n�� ���� �������� ���������.�";
                        break;
                    case 8:
                        StartCoroutine(ShowTask());
                        text = "����������� �� ������ 50 �����";
                        break;
                    case 9:
                        StartCoroutine(ShowInstructorText());
                        text = "������� ��� ���������� ������ �����\n��� ���������� ������������� �������.�";
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

            Debug.LogError("����������� ����� ���������! " + reason);
        }
    }

    public override void CompletedGame()
    {
        throw new System.NotImplementedException();
    }
}