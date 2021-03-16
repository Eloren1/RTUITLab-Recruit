using UnityEngine;

public class PlaneVisuals : MonoBehaviour
{
    [Header("Крылья")]
    [SerializeField] private GameObject rudder; // Используются для изменения Yaw
    private float maxYawAngle = 30f;

    [SerializeField] private GameObject elevator_r; // Используются для изменения Pitch
    [SerializeField] private GameObject elevator_l; // Используются для изменения Pitch
    private float maxPitchAngle = 25f;

    [SerializeField] private GameObject aileron_r; // Используются для изменения Roll
    [SerializeField] private GameObject aileron_l; // Используются для изменения Roll
    private float maxRollAngle = 25f;

    [SerializeField] private GameObject flaps_r;
    [SerializeField] private GameObject flaps_l;
    private float maxFlapAngle = 25f;

    [Header("Кабина")]
    [SerializeField] private GameObject cockpitStick;
    private float maxStickAngle = 12f;

    [Header("Ливрея")]
    [SerializeField] private MeshRenderer[] texturedParts; // Детали, в которых нужно поменять текстуру на выбранную
    [SerializeField] private Material[] textures;

    private void Awake()
    {
        foreach (var part in texturedParts)
        {
            part.material = textures[PlayerPrefs.GetInt("Plane")];
        }
    }

    public void UpdateVisuals(float yaw, float pitch, float roll, float flaps)
    {
        // Визуальное вращение винта происходит в Engine.cs

        rudder.transform.localRotation = Quaternion.Euler(0, maxYawAngle * -yaw, 0);

        elevator_r.transform.localRotation = Quaternion.Euler(maxPitchAngle * -pitch, 0, 0);
        elevator_l.transform.localRotation = Quaternion.Euler(maxPitchAngle * -pitch, 0, 0);

        aileron_r.transform.localRotation = Quaternion.Euler(maxRollAngle * roll, -6.604f, 0);
        aileron_l.transform.localRotation = Quaternion.Euler(maxRollAngle * roll, -173.396f, 0);

        flaps_r.transform.localRotation = Quaternion.Euler(maxFlapAngle * -flaps, -6.998f, 0);
        flaps_l.transform.localRotation = Quaternion.Euler(maxFlapAngle * flaps, -173.002f, 0);

        cockpitStick.transform.localRotation = Quaternion.Euler(maxStickAngle * pitch, 0, maxStickAngle * -roll);
    }
}
