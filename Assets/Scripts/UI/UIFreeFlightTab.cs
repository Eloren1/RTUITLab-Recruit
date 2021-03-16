using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIFreeFlightTab : MonoBehaviour
{
    [SerializeField] private Dropdown weatherDropdown;

    private void Start()
    {
        ChangeWeather();
    }

    public void Play(int level)
    {
        if (UISoundManager.Instance != null)
            UISoundManager.Instance.PlayClickSound();

        PlayerPrefs.SetInt("Gamemode", 1);
        PlayerPrefs.SetInt("Level", level);

        SceneManager.LoadScene(1);
    }

    public void ChangeWeather()
    {
        PlayerPrefs.SetInt("Weather", weatherDropdown.value);
    }
}
