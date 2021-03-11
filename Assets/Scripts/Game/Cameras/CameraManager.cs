using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera[] switchCameras;
    private int currentCamera = -1;

    public void Awake()
    {
        foreach (var camera in switchCameras)
        {
            if (camera != null)
            {
                camera.gameObject.SetActive(false);
            }
        }

        NextCamera();
    }

    public void NextCamera()
    {
        if (currentCamera >= 0)
            if (switchCameras[currentCamera] != null)
                switchCameras[currentCamera].gameObject.SetActive(false);

        currentCamera++;
        currentCamera %= switchCameras.Length;

        if (switchCameras[currentCamera] != null)
            switchCameras[currentCamera].gameObject.SetActive(true);
        else
            NextCamera();
    }
}
