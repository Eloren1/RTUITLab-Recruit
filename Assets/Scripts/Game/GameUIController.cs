using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    [Header("Игровой интерфейс")]
    [SerializeField] private GameObject timePanel;
    [SerializeField] private Text timeOutput;
    [SerializeField] private GameObject taskPanel;
    [SerializeField] private Text taskOutput;

    [SerializeField] private Animator menuAnim;

    [Header("Информация самолета")]
    [SerializeField] private Slider thrustSlider;
    [SerializeField] private Text infoOutput;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void UpdatePlaneInfo(float thrust, int rpm, int knots, int altitude)
    {
        thrustSlider.value = thrust;

        infoOutput.text = $"{rpm} RPM\n" +
            $"{knots} KNOTS\n" +
            $"{altitude} FEET";
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

    public void ToggleMenu()
    {
        menuAnim.SetBool("Opened", !menuAnim.GetBool("Opened"));
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
