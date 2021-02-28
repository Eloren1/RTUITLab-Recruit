using UnityEngine;
using UnityEngine.SceneManagement;

public class UITutorialTab : MonoBehaviour
{
    public void Play(int level)
    {
        PlayerPrefs.SetInt("Gamemode", 2);
        PlayerPrefs.SetInt("Level", level);

        SceneManager.LoadScene(1);
    }
}
