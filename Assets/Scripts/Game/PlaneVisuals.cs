using UnityEngine;

public class PlaneVisuals : MonoBehaviour
{
    [Header("������")]
    [SerializeField] private GameObject rudder; // ������������ ��� ��������� Yaw
    private float maxYawAngle = 30f;

    [SerializeField] private GameObject elevator_r; // ������������ ��� ��������� Pitch
    [SerializeField] private GameObject elevator_l; // ������������ ��� ��������� Pitch
    private float maxPitchAngle = 25f;

    [SerializeField] private GameObject aileron_r; // ������������ ��� ��������� Roll
    [SerializeField] private GameObject aileron_l; // ������������ ��� ��������� Roll
    private float maxRollAngle = 25f;

    [SerializeField] private GameObject flaps_r;
    [SerializeField] private GameObject flaps_l;
    private float maxFlapAngle = 25f;

    [Header("������")]
    [SerializeField] private GameObject cockpitStick;
    private float maxStickAngle = 12f;

    [Header("������")]
    [SerializeField] private MeshRenderer[] texturedParts; // ������, � ������� ����� �������� �������� �� ���������
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
        // ���������� �������� ����� ���������� � Engine.cs

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
