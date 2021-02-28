using UnityEngine;
using UnityEngine.SceneManagement;

public class UIFreeFlightTab : MonoBehaviour
{
    public void Play(int level)
    {
        PlayerPrefs.SetInt("Gamemode", 1);
        PlayerPrefs.SetInt("Level", level);

        SceneManager.LoadScene(1);
    }
}
