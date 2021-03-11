using UnityEngine;

public class Engine : MonoBehaviour
{
    [Header("Параметры")]
    [SerializeField] private float power = 51280f;
    [SerializeField] private float liftingForce = 1000f;
    public bool IsWorking = true;
    [SerializeField] private float stallSpeed = 40f; // Скорость в узлах, после которой самолет будет падать
    [SerializeField] private float fallForce = 7000f;

    [Header("Управление")]
    private float currentThrust;
    public float rpm;
    private float maxRpm;
    private float rpmForce;
    public float SpeedAffect;
    private float maxMagnitude = 150f;

    [Header("Компоненты")]
    [SerializeField] private GameObject prop;

    [Header("Утилиты")]
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
            currentThrust = Mathf.Lerp(currentThrust, thrust, 1f / 200f);
            currentThrust = Mathf.Clamp(currentThrust, 0f, 1f);

            rpm = currentThrust * power / 5 + magnitude;

            rpmForce = rpm * 100;

            // Движение самолета вперед,
            // чем выше скорость, тем меньше добавляем силы
            rb.AddRelativeForce(Vector3.forward * (rpmForce - magnitude * 500f));

            if (knots < stallSpeed)
            {
                float stallAffect = stallSpeed - knots;

                float zAngle = transform.eulerAngles.z > 180 ? transform.eulerAngles.z - 360 : transform.eulerAngles.z;
                float xAngle = transform.eulerAngles.x > 180 ? transform.eulerAngles.x + 90 - 360 : transform.eulerAngles.x + 90;

                // Самолет будет падать набок в зависимости от угла крена
                rb.AddRelativeTorque(Vector3.forward * 1500 * stallAffect * (zAngle > 0 ? 1 : -1));

                rb.AddForce(-Vector3.up * stallAffect * fallForce);
            }
        } else
        {
            rb.velocity *= 0.98f;

            currentThrust = 0;
            rpm = 0;
        }

        prop.transform.Rotate(Vector3.forward, rpm * 6 * Time.deltaTime);

        rb.AddRelativeForce(Vector3.up * magnitude * liftingForce);
    }

    private void OnDrawGizmos()
    {
        if (rb != null)
        {
            // Текущее направление самолета
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + rb.velocity / 5);

            // Направление вперед
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 15);
        }
    }
}
