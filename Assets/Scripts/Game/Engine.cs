using UnityEngine;

public class Engine : MonoBehaviour
{
    [Header("���������")]
    [SerializeField] private float power = 51280f;
    [SerializeField] private float liftingForce = 1000f;

    [Header("����������")]
    private float currentThrust;
    private float rpm;
    private float maxRpm;
    private float rpmForce;
    public float SpeedAffect;
    private float maxMagnitude = 150f;

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
        SpeedAffect = (magnitude / maxMagnitude) % 1;

        currentThrust = Mathf.Lerp(currentThrust, thrust, 1f / 200f);
        currentThrust = Mathf.Clamp(currentThrust, 0f, 1f);

        rpm = currentThrust * power / 10;
        // Debug.Log(rpm);

        rpmForce = rpm * 20 * 10;

        prop.transform.Rotate(-Vector3.forward * (rpm + magnitude / 30f));

        // ��� >70% �������� ��������� ������� ��������� ����
        // ��� ������� �������� � ������ ����
        rb.AddRelativeForce(Vector3.up * Mathf.Clamp(((rpmForce / maxRpm) - 0.7f), -0.3f, 0.3f) * magnitude * liftingForce);

        // �������� �������� ������,
        // ��� ���� ��������, ��� ������ ��������� ����
        rb.AddRelativeForce(Vector3.forward * (rpmForce - magnitude * 400f));
    }

    private void OnDrawGizmos()
    {
        if (rb != null)
        {
            // ������� ����������� ��������
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + rb.velocity / 5);

            // ����������� ������
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 15);
        }
    }
}
