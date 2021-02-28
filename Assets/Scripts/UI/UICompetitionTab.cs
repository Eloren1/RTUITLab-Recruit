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

                mapTimes[i].text = "ÂÐÅÌß: " + ((seconds > 9) ? "" : "0") + seconds +
                    ((minutes > 9) ? "" : "0") + minutes + ((hours > 9) ? "" : "0") + hours;
            }
            else
            {
                mapTimes[i].text = "ÂÐÅÌß: --:--:--";
            }
        }
    }

    public void Play(int level)
    {
        PlayerPrefs.SetInt("Gamemode", 0);
        PlayerPrefs.SetInt("Level", level);

        SceneManager.LoadScene(1);
    }
}
