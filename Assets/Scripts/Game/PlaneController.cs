using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlaneVisuals))]
[RequireComponent(typeof(Chassis))]
[RequireComponent(typeof(Engine))]
public class PlaneController : MonoBehaviour
{
    [Header("Параметры")]


    [Header("Управление")]
    private float thrust;
    private float yaw;
    [SerializeField] private float yawForce = 20f;
    private float pitch;
    [SerializeField] private float pitchForce = 12f;
    private float roll;
    [SerializeField] private float rollForce = 300f;
    private float flaps;
    [SerializeField] private float flapsForce = 1000f;
    [SerializeField] private float changingSpeed = 0.1f;
    private float magnitude;
    
    [Header("Компоненты")]
    private Chassis chassis;
    private Engine engine;

    [Header("Утилиты")]
    private Rigidbody rb;
    private PlaneVisuals planeVisuals;
    private Inputs inputs;

    private void Awake()
    {
        chassis = GetComponent<Chassis>();
        engine = GetComponent<Engine>();

        planeVisuals = GetComponent<PlaneVisuals>();
        rb = GetComponent<Rigidbody>();

        Time.timeScale = 0.1f;
    }

    public void AssignInputs(Inputs inputs) { this.inputs = inputs; }

    public void SetStartValues(Vector3 velocity, float rpm, float thrust)
    {
        Debug.LogError("Start values are not assinged");
    }

    private void Update()
    {
        if (inputs != null)
        {
            if (inputs.ToggleChassis())
            {
                chassis.ToggleChassis();
            }

            planeVisuals.UpdateVisuals(yaw, pitch, roll, flaps);
        } else
        {
            Debug.LogError("Inputs are not assigned");
        }
    }

    private void FixedUpdate()
    {
        magnitude = transform.InverseTransformDirection(rb.velocity).z;

        if (inputs != null)
        {
            // При отсутствии ввода значения не меняются
            thrust = Mathf.Clamp(thrust + inputs.ThrustNormalized() * 0.01f, 0f, 1f);
            flaps = Mathf.Clamp(flaps + inputs.FlapsNormalized() * 0.02f, 0f, 1f);

            // При отсутствии ввода значения возвращаются к нулю
            yaw = Mathf.Lerp(yaw, inputs.YawNormalized(), changingSpeed);
            pitch = Mathf.Lerp(pitch, inputs.PitchNormalized(), changingSpeed);
            roll = Mathf.Lerp(roll, inputs.RollNormalized(), changingSpeed);


            engine.AddForce(thrust, magnitude);

            AddYawForce();
            AddRollForce();
            AddPitchForce();

            // Debug.Log(rb.velocity.magnitude * 3.6f * 0.53996f); // Скорость в Knots

            AddFlapsLiftingForce();
        }

        // Уменьшение вращательной инерции
        rb.angularVelocity *= 0.98f;
    }

    private void AddYawForce()
    {
        // rb.AddRelativeTorque(Vector3.up * yaw * yawForce);
    }

    private void AddRollForce()
    {
        rb.AddRelativeTorque(Vector3.forward * -roll * magnitude * rollForce);
    }

    private void AddPitchForce()
    {
        rb.AddRelativeTorque(Vector3.right * pitch * magnitude * pitchForce);
    }

    private void AddFlapsLiftingForce()
    {
        rb.AddRelativeForce(Vector3.up * flaps * magnitude * flapsForce);
    }

    public void OnTriggerEnter(Collider other)
    {
        Circle circle = other.GetComponent<Circle>();
        if (circle != null) { circle.Collected(); }
    }
}
