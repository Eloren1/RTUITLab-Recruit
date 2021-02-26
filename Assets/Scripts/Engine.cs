using UnityEngine;

public class Engine : MonoBehaviour
{
    [Header("���������")]
    [SerializeField] private float power = 4000f;
    [SerializeField] private float liftingForce = 1000f;

    [Header("����������")]
    private float rpm;
    private float maxRpm;

    [Header("����������")]
    [SerializeField] private GameObject prop;

    [Header("�������")]
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        maxRpm = power * 20;
    }

    public void AddForce(float thrust, float magnitude)
    {
        /*
         * TODO: �������� stall, �������� ���� ����� �������� �� ���� ������� �� ��������� � ������
         */

        // ��� ���� ��������, ��� ������ ��������� ����

        // TODO: ������� ������������� �����
        rpm = thrust * power * 20;

        prop.transform.Rotate(-Vector3.forward * rpm);

        // ��� 70% �������� ������� ����� ������ �����,
        // ��� ������� �������� � ������ ����
        rb.AddRelativeForce(Vector3.up * Mathf.Clamp(((rpm / maxRpm) - 0.7f), -0.3f, 0.3f) * magnitude * liftingForce);

        // �������� �������� ������
        rb.AddRelativeForce(Vector3.forward * (rpm - magnitude * 300f));


        float angle = -Vector3.SignedAngle(Vector3.forward, transform.InverseTransformDirection(rb.velocity), Vector3.up);
        Debug.Log(angle);

        // TODO: angle ����� ������������ ���� ��� ������ � ������ ������� / � ������������ ���������

        // ������������� ������� �� ������� ��� ��������
        rb.AddRelativeForce(Vector3.up * 20000 * (rpm / maxRpm) * angle);

        // ���������� �������� ������ ��-�� ������������� �������
        rb.AddRelativeForce(-Vector3.forward * Mathf.Abs((rpm / maxRpm) * angle) * 100);
    }
}
