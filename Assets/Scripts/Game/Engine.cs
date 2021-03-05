using UnityEngine;

public class Engine : MonoBehaviour
{
    [Header("Параметры")]
    [SerializeField] private float power = 51280f;
    [SerializeField] private float liftingForce = 1000f;

    [Header("Управление")]
    private float currentThrust;
    private float rpm;
    private float maxRpm;
    private float rpmForce;
    private float speedAffect;
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

        // При 70% мощности самолет будет лететь прямо,
        // при меньшей мощности — лететь вниз
        rb.AddRelativeForce(Vector3.up * Mathf.Clamp(((rpmForce / maxRpm) - 0.7f), -0.3f, 0.3f) * magnitude * liftingForce);

        // Движение самолета вперед,
        // чем выше скорость, тем меньше добавляем силы
        rb.AddRelativeForce(Vector3.forward * (rpmForce - magnitude * 400f));

        var velocity = transform.InverseTransformDirection(rb.velocity);
        velocity.x = 0;
        rb.velocity = transform.TransformPoint(velocity);

        float angle = Vector3.SignedAngle(transform.forward, transform.InverseTransformDirection(rb.velocity), new Vector3(1, 0, 0));
        Debug.Log(angle);

        // Сопротивление воздуха от крыльев под наклоном,
        // спустя время самолет будет выравниваться и лететь прямо
        rb.AddRelativeForce(Vector3.up * 120000 * speedAffect * angle);

        // Уменьшение скорости вперед из-за сопротивления воздуха
        rb.AddRelativeForce(-Vector3.forward * Mathf.Abs(speedAffect * angle) * 2500);
    }

    private void OnDrawGizmos()
    {
        if (rb != null)
        {
            // Текущее направление самолета
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.InverseTransformDirection(rb.velocity / 5));

            // Направление вперед
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 15);
        }
    }
}
