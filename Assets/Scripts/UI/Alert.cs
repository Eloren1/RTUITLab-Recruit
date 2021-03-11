using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))] 
public class Alert : MonoBehaviour
{
    public static Alert Instance;
    private Animator anim;

    [SerializeField] private Text textOutput;

    private void Awake()
    {
        Instance = this;

        anim = GetComponent<Animator>();

        anim.Play("Alert FadeInOut", 0, 1f); // ��������� �������� � �����
    }

    public void ShowAlert(string text)
    {
        textOutput.text = text;

        anim.Play("Alert FadeInOut", 0, 0f); // ��������� �������� � ������
    }
}
