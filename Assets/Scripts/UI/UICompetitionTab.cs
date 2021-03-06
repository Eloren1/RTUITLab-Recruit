using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UICompetitionTab : MonoBehaviour
{
    [SerializeField] private Text[] mapTimes;

    private void Start()
    {
        for (int i = 0; i < mapTimes.Length; i++)
        {
            int time = PlayerPrefs.GetInt("Competition " + i);

            if (time != 0)
            {
                int seconds = time % 60;
                int minutes = (time / 60) % 60;
                int hours = (time / 60) / 60;

                mapTimes[i].text = "�����: " + ((hours > 9) ? "" : "0") + hours + ":" +
                    ((minutes > 9) ? "" : "0") + minutes + ":" + ((seconds > 9) ? "" : "0") + seconds;
            }
            else
            {
                mapTimes[i].text = "�����: --:--:--";
            }
        }
    }

    public void Play(int level)
    {
        if (UISoundManager.Instance != null)
            UISoundManager.Instance.PlayClickSound();

        PlayerPrefs.SetInt("Gamemode", 0);
        PlayerPrefs.SetInt("Level", level);

        switch (level)
        {
            case 0:
                PlayerPrefs.SetInt("Weather", 0);
                break;
            case 1:
                PlayerPrefs.SetInt("Weather", 0);
                break;
            case 2:
                PlayerPrefs.SetInt("Weather", 2);
                break;
        }

        SceneManager.LoadScene(1);
    }
}
