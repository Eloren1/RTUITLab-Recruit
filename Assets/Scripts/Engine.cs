using UnityEngine;

public class Engine : MonoBehaviour
{
    [Header("Параметры")]
    [SerializeField] private float power = 4000f;
    [SerializeField] private float liftingForce = 1000f;

    [Header("Управление")]
    private float rpm;
    private float maxRpm;

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
        /*
         * TODO: Добавить stall, мощность силы будет зависеть от угла наклона по сравнению с землей
         */

        // Чем выше скорость, тем меньше добавляем силы

        // TODO: Плавное раскручивание винта
        rpm = thrust * power * 20;

        prop.transform.Rotate(-Vector3.forward * rpm);

        // При 70% мощности самолет будет лететь прямо,
        // при меньшей мощности — лететь вниз
        rb.AddRelativeForce(Vector3.up * Mathf.Clamp(((rpm / maxRpm) - 0.7f), -0.3f, 0.3f) * magnitude * liftingForce);

        // Движение самолета вперед
        rb.AddRelativeForce(Vector3.forward * (rpm - magnitude * 300f));


        float angle = -Vector3.SignedAngle(Vector3.forward, transform.InverseTransformDirection(rb.velocity), Vector3.up);
        Debug.Log(angle);

        // TODO: angle имеет неправильный знак при полете в другую сторону / в перевернутом состоянии

        // Сопротивление воздуха от крыльев под наклоном
        rb.AddRelativeForce(Vector3.up * 20000 * (rpm / maxRpm) * angle);

        // Уменьшение скорости вперед из-за сопротивления воздуха
        rb.AddRelativeForce(-Vector3.forward * Mathf.Abs((rpm / maxRpm) * angle) * 100);
    }
}
