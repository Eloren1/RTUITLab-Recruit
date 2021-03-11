using UnityEngine;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{
    [Header("�������")]
    [SerializeField] private Text graphicsOutput;
    private string[] outputTexts = { "������", "�������", "�������", "������" };

    [Header("���������")]
    [SerializeField] private Image guidesImage;
    [SerializeField] private Text guidesOutput;
    [SerializeField] private Sprite guidesOn;
    [SerializeField] private Sprite guidesOff;

    private void Start()
    {
        int guides = PlayerPrefs.GetInt("Guides");
        ChangeGuides(guides == 1);

        int graphics = PlayerPrefs.GetInt("Graphics");
        ChangeGraphics(graphics);
    }

    public void OnClickGuides()
    {
        int guides = PlayerPrefs.GetInt("Guides");
        if (guides == 1) // ���� ��������, ���������
        {
            guides = 0;
            ChangeGuides(false);
        }
        else // ���� ���������, ��������
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

            guidesOutput.text = "���";
            guidesImage.sprite = guidesOn;
        }
        else
        {
            PlayerPrefs.SetInt("Guides", 0);

            guidesOutput.text = "����";
            guidesImage.sprite = guidesOff;
        }
    }

    public void OnClickGraphics()
    {
        int graphics = PlayerPrefs.GetInt("Graphics");
        graphics++;
        graphics %= 4;

        ChangeGraphics(graphics);
    }

    private void ChangeGraphics(int graphics)
    {
        PlayerPrefs.SetInt("Graphics", graphics);

        graphicsOutput.text = outputTexts[graphics];
    }

    public void OnClickControlType()
    {
        // TODO: ��� ���������� ����� ������� ���������� ��������� ����������� ������ �����.

        Alert.Instance.ShowAlert("������ �������� ������ ���������� � ����������");
    }
}
