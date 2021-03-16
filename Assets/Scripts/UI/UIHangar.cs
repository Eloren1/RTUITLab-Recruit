using UnityEngine;
using UnityEngine.UI;

public class UIHangar : MonoBehaviour
{
    [SerializeField] private GameObject selectButton;
    [SerializeField] private GameObject selected;

    [SerializeField] private Animator cameraAnim;

    private int currentPlane;
    [SerializeField] private int totalPlanes;

    private void OnEnable()
    {
        currentPlane = PlayerPrefs.GetInt("Plane");

        CheckSelected();
        MoveCamera(currentPlane);
    }

    public void NextPlane()
    {
        if (UISoundManager.Instance != null)
            UISoundManager.Instance.PlayClickSound();

        currentPlane++;

        if (currentPlane == totalPlanes)
        {
            currentPlane -= totalPlanes;
        }

        MoveCamera(currentPlane);

        CheckSelected();
    }

    public void PrevPlane()
    {
        if (UISoundManager.Instance != null)
            UISoundManager.Instance.PlayClickSound();

        currentPlane--;

        if (currentPlane == -1)
        {
            currentPlane += totalPlanes;
        }

        MoveCamera(currentPlane);

        CheckSelected();
    }

    private void MoveCamera(int id)
    {
        // Двигаем камеру в сторону нужного самолета
        cameraAnim.SetInteger("Plane", id);
    }

    public void Select()
    {
        if (UISoundManager.Instance != null)
            UISoundManager.Instance.PlayClickSound();

        PlayerPrefs.SetInt("Plane", currentPlane);

        CheckSelected();
    }

    public void ReturnToMainMenu()
    {
        if (UISoundManager.Instance != null)
            UISoundManager.Instance.PlayClickSound();

        // Возвращаем камеру на место
        cameraAnim.SetInteger("Plane", -1);

        FindObjectOfType<UIControllerMainMenu>().ReturnToMainMenuSilent();
    }

    private void CheckSelected()
    {
        if (currentPlane == PlayerPrefs.GetInt("Plane"))
        {
            selected.SetActive(true);
            selectButton.SetActive(false);
        }
        else
        {
            selected.SetActive(false);
            selectButton.SetActive(true);
        }
    }
}
