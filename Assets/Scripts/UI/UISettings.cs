using UnityEngine;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{
    [Header("Графика")]
    [SerializeField] private Text graphicsOutput;
    private string[] outputTexts = { "НИЗКАЯ", "СРЕДНЯЯ", "ВЫСОКАЯ" };

    [Header("Подсказки")]
    [SerializeField] private Image guidesImage;
    [SerializeField] private Text guidesOutput;
    [SerializeField] private Sprite guidesOn;
    [SerializeField] private Sprite guidesOff;

    [Header("Утилиты")]
    [SerializeField] private UIControllerMainMenu mainUI;

    private void Start()
    {
        int guides = PlayerPrefs.GetInt("Guides");
        ChangeGuides(guides == 1);

        int graphics = PlayerPrefs.GetInt("Graphics");
        graphicsOutput.text = outputTexts[graphics];
    }

    public void OnClickGuides()
    {
        if (UISoundManager.Instance != null)
            UISoundManager.Instance.PlayClickSound();

        int guides = PlayerPrefs.GetInt("Guides");
        if (guides == 1) // Было включено, выключаем
        {
            guides = 0;
            ChangeGuides(false);
        }
        else // Было выключено, включаем
        {
            guides = 1;
            ChangeGuides(true);
        }
    }

    private void ChangeGuides(bool on)
    {
        if (on)
        {
            PlayerPrefs.SetInt("Guides", 1);

            guidesOutput.text = "ВКЛ";
            guidesImage.sprite = guidesOn;
        }
        else
        {
            PlayerPrefs.SetInt("Guides", 0);

            guidesOutput.text = "ВЫКЛ";
            guidesImage.sprite = guidesOff;
        }
    }

    public void OnClickGraphics()
    {
        if (UISoundManager.Instance != null)
            UISoundManager.Instance.PlayClickSound();

        int graphics = PlayerPrefs.GetInt("Graphics");
        graphics++;
        graphics %= outputTexts.Length;

        SetGraphics(graphics);
    }

    private void SetGraphics(int graphics)
    {
        PlayerPrefs.SetInt("Graphics", graphics);
        graphicsOutput.text = outputTexts[graphics];

        mainUI.ChangeGraphics();
    }

    public void OnClickControlType()
    {
        if (UISoundManager.Instance != null)
            UISoundManager.Instance.PlayClickSound();

        // TODO: При добавлении новых режимов управления добавлять возможность выбора здесь

        Alert.Instance.ShowAlert("СЕЙЧАС ДОСТУПНО ТОЛЬКО УПРАВЛЕНИЕ С КЛАВИАТУРЫ");
    }
}
