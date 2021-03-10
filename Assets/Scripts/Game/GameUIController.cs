using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private Transform target;

    [Header("Игровой интерфейс")]
    [SerializeField] private GameObject timePanel;
    [SerializeField] private Text timeOutput;
    [SerializeField] private GameObject taskPanel;
    [SerializeField] private Text taskOutput;
    [SerializeField] private GameObject subTaskPanel;
    [SerializeField] private Text subTaskOutput;

    [SerializeField] private RectTransform compass;

    [SerializeField] private Animator menuAnim;

    [Header("Информация самолета")]
    [SerializeField] private Slider thrustSlider;
    [SerializeField] private Text infoOutput;

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }

        if (target != null)
        {
            compass.rotation = Quaternion.Euler(0, 0, target.eulerAngles.y);
        }
    }

    public void UpdatePlaneInfo(float thrust, int rpm, int knots, int altitude)
    {
        thrustSlider.value = thrust;

        infoOutput.text = $"{rpm} RPM\n" +
            $"{knots} УЗЛОВ\n" +
            $"{altitude} ФУТОВ";
    }

    public void SetTime(string time)
    {
        if (time.Length > 0)
        {
            timePanel.SetActive(true);
            timeOutput.text = time;
        }
        else
        {
            timePanel.SetActive(false);
            timeOutput.text = "";
        }
    }

    public void SetTask(string task)
    {
        if (task.Length > 0)
        {
            taskPanel.SetActive(true);
            taskOutput.text = task;
        }
        else
        {
            taskPanel.SetActive(false);
            taskOutput.text = "";
        }
    }

    public void SetSubTask(string task)
    {
        if (task.Length > 0)
        {
            subTaskPanel.SetActive(true);
            subTaskOutput.text = task;
        }
        else
        {
            subTaskPanel.SetActive(false);
            subTaskOutput.text = "";
        }
    }

    public void ToggleMenu()
    {
        menuAnim.SetBool("Opened", !menuAnim.GetBool("Opened"));

        if (menuAnim.GetBool("Opened"))
        {
            Cursor.visible = true;
        } else
        {
            Cursor.visible = false;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
