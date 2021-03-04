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
    private float maxMagnitude = 120f;

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
        Debug.Log(rpm);

        rpmForce = rpm * 20 * 10;

        prop.transform.Rotate(-Vector3.forward * (rpm + magnitude / 30f));

        // При 70% мощности самолет будет лететь прямо,
        // при меньшей мощности — лететь вниз
        rb.AddRelativeForce(Vector3.up * Mathf.Clamp(((rpmForce / maxRpm) - 0.7f), -0.3f, 0.3f) * magnitude * liftingForce);

        // Движение самолета вперед,
        // чем выше скорость, тем меньше добавляем силы
        rb.AddRelativeForce(Vector3.forward * (rpmForce - magnitude * 400f));

        float angle = Vector3.SignedAngle(transform.forward, rb.velocity, Vector3.one);
        // Debug.Log(angle);

        // Сопротивление воздуха от крыльев под наклоном,
        // спустя время самолет будет выравниваться и лететь прямо
        rb.AddRelativeForce(Vector3.up * 120000 * speedAffect * angle);

        // Уменьшение скорости вперед из-за сопротивления воздуха
        rb.AddRelativeForce(-Vector3.forward * Mathf.Abs(speedAffect * angle) * 500);
    }
}
