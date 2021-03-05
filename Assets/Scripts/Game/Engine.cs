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
    private float speedAffect;
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
        speedAffect = (magnitude / maxMagnitude) % 1;

        currentThrust = Mathf.Lerp(currentThrust, thrust, 1f / 250f);
        currentThrust = Mathf.Clamp(currentThrust, 0f, 1f);
        // Debug.Log(currentThrust + " <- " + thrust);

        rpm = currentThrust * power / 10;
        // Debug.Log(rpm);

        rpmForce = rpm * 20 * 10;

        prop.transform.Rotate(-Vector3.forward * (rpm + magnitude / 30f));

        // ��� 70% �������� ������� ����� ������ �����,
        // ��� ������� �������� � ������ ����
        rb.AddRelativeForce(Vector3.up * Mathf.Clamp(((rpmForce / maxRpm) - 0.7f), -0.3f, 0.3f) * magnitude * liftingForce);

        // �������� �������� ������,
        // ��� ���� ��������, ��� ������ ��������� ����
        rb.AddRelativeForce(Vector3.forward * (rpmForce - magnitude * 400f));

        var velocity = transform.InverseTransformDirection(rb.velocity);
        velocity.x = 0;
        rb.velocity = transform.TransformPoint(velocity);

        float angle = Vector3.SignedAngle(transform.forward, transform.InverseTransformDirection(rb.velocity), new Vector3(1, 0, 0));
        Debug.Log(angle);

        // ������������� ������� �� ������� ��� ��������,
        // ������ ����� ������� ����� ������������� � ������ �����
        rb.AddRelativeForce(Vector3.up * 120000 * speedAffect * angle);

        // ���������� �������� ������ ��-�� ������������� �������
        rb.AddRelativeForce(-Vector3.forward * Mathf.Abs(speedAffect * angle) * 2500);
    }

    private void OnDrawGizmos()
    {
        if (rb != null)
        {
            // ������� ����������� ��������
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.InverseTransformDirection(rb.velocity / 5));

            // ����������� ������
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 15);
        }
    }
}
