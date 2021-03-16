using UnityEngine;
using UnityEngine.SceneManagement;

public class UITutorialTab : MonoBehaviour
{
    public void Play(int level)
    {
        if (UISoundManager.Instance != null)
            UISoundManager.Instance.PlayClickSound();

        PlayerPrefs.SetInt("Gamemode", 2);
        PlayerPrefs.SetInt("Level", level);

        switch (level)
        {
            case 0:
                PlayerPrefs.SetInt("Weather", 0);
                break;
            case 1:
                PlayerPrefs.SetInt("Weather", 2);
                break;
            case 2:
                PlayerPrefs.SetInt("Weather", 0);
                break;
        }

        SceneManager.LoadScene(1);
    }
}
