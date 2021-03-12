using UnityEngine;

public class Engine : MonoBehaviour
{
    [Header("���������")]
    [SerializeField] private float power = 51280f;
    [SerializeField] private float enginePowerModifier = 1.8f;
    [SerializeField] private float rpmAddingSpeed = 33f;
    public bool IsWorking = true;
    
    [Header("������� �� ������ ��������")]
    [Tooltip("�������� � �����, ����� ������� ������� ����� ������")]
    [SerializeField] private float stallSpeed = 40f;
    [SerializeField] private float rotationForce = 4000f;
    [SerializeField] private float fallForce = 15000f;

    [Header("����������")]
    private float currentThrust;
    public float rpm;
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

    public void SetCurrentThrust(float thrust)
    {
        currentThrust = thrust;
    }

    public void AddForce(float thrust, float magnitude, float knots)
    {
        SpeedAffect = (magnitude / maxMagnitude) % 1;

        if (IsWorking)
        {
            currentThrust = Mathf.Lerp(currentThrust, thrust, rpmAddingSpeed / 10000);
            currentThrust = Mathf.Clamp(currentThrust, 0f, 1f);

            rpm = currentThrust * power / 5 + magnitude;

            rpmForce = rpm * 100;

            // Debug.Log(magnitude);

            rb.AddRelativeForce(Vector3.forward * rpmForce * enginePowerModifier);

            if (knots < stallSpeed)
            {
                float stallAffect = stallSpeed - knots;

                float zAngle = transform.eulerAngles.z > 180 ? transform.eulerAngles.z - 360 : transform.eulerAngles.z;

                // ������� ����� ����������� � �������
                rb.AddRelativeTorque(Vector3.forward * rotationForce * stallAffect * (zAngle > 0 ? 1 : -1));

                rb.AddForce(-Vector3.up * stallAffect * fallForce);
            }
        } else
        {
            rb.velocity *= 0.98f;

            currentThrust = 0;
            rpm = 0;
        }

        prop.transform.Rotate(Vector3.forward, rpm * 6 * Time.deltaTime);
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
