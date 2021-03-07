using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIFreeFlightTab : MonoBehaviour
{
    [SerializeField] private Dropdown weatherDropdown;

    public void Play(int level)
    {
        PlayerPrefs.SetInt("Gamemode", 1);
        PlayerPrefs.SetInt("Level", level);

        SceneManager.LoadScene(1);
    }

    public void ChangeWeather()
    {
        PlayerPrefs.SetInt("Weather", weatherDropdown.value);
    }
}
