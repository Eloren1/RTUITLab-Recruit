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

        // ���������� ������� � ������ �����
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
                        text = "��� ��������� ���������� ������!\n������� ���� ����������.�";
                        break;
                    case 1:
                        StartCoroutine(ShowTask());
                        text = "����������� [W] � [S] ��� ��������� ���� �������";
                        break;
                    case 2:
                        StartCoroutine(ShowInstructorText());
                        text = "����������. �� �������� ������ �������.\n����� �� ������ �������� ��� ���������.�";
                        break;
                    case 3:
                        StartCoroutine(ShowInstructorText());
                        text = "��� �� ������ 5000 �����.\n��������� �� ������ � 6000 �����.�";
                        break;
                    case 4:
                        StartCoroutine(ShowTask());
                        text = "����������� �� ������ � 6000 �����";
                        break;
                    case 5:
                        StartCoroutine(ShowInstructorText());
                        text = "�� ������ ��� �����\n�������� ����������� ������.�";
                        break;
                    case 6:
                        StartCoroutine(ShowTask());
                        text = "����������� [A] � [D] ��� ��������� ���� �����";
                        break;
                    case 7:
                        StartCoroutine(ShowInstructorText());
                        text = "������� ����� ������ ���� �������,\n����� ������ ������������.�";
                        break;
                    case 8:
                        StartCoroutine(ShowTask());
                        text = "����������� [W] � [S] ��� ��������� ���� �������";
                        break;
                    case 9:
                        StartCoroutine(ShowInstructorText());
                        text = "������ ������� ����������� ������ �� 180 ��������.�";
                        break;
                    case 10:
                        StartCoroutine(ShowInstructorText());
                        text = "�������� ������� ��������� � ������� ������.�";
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
                        text = "����� �����������, �������� �������� � ���������.�";
                        break;
                    case 14:
                        StartCoroutine(ShowTask());
                        text = "����������� [R] � [F] ��� ��������� ��������";
                        break;
                    case 15:
                        StartCoroutine(ShowInstructorText());
                        text = "��������. ��� ��������� �������\n������� ������� ���� ����.�";
                        break;
                    case 16:
                        StartCoroutine(ShowTask());
                        text = "����������� �� ������ � 9000 �����";
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

            // �� ���������� �� ������ ����� 4000 �����
        }
    }

    public override void CompletedGame()
    {
        throw new System.NotImplementedException();
    }
}