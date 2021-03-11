using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlaneVisuals))]
[RequireComponent(typeof(Chassis))]
[RequireComponent(typeof(Engine))]
[RequireComponent(typeof(Brakes))]
public class PlaneController : MonoBehaviour
{
    [Header("Параметры")]
    [SerializeField] private float yawForce = 2500f;
    [SerializeField] private float pitchForce = 3200f;
    [SerializeField] private float rollForce = 7000f;
    [SerializeField] private float flapsForce = 5000f;
    [SerializeField] private float changingSpeed = 0.1f;

    [SerializeField] private bool warnAboutBankAngle = false;
    [SerializeField] private float bankAngleZ = 75f;
    [SerializeField] private bool warnAboutPullUp = true;
    [SerializeField] private float pullUpAngle = 55f;
    [Tooltip("Высота в футах")]
    [SerializeField] private float pullUpHeight = 2000f;

    [Header("Управление")]
    private float thrust;
    private float yaw;
    private float pitch;
    private float roll;
    private float flaps;
    private float magnitude;
    public float Magnitude { get { return magnitude; } }
    private float angle;
    
    [Header("Компоненты")]
    private Chassis chassis;
    private Engine engine;
    private Brakes brakes;
    private PlaneVisuals planeVisuals;

    [Header("Утилиты")]
    [SerializeField] private PlaneSound sound;
    private Rigidbody rb;
    private Inputs inputs;
    private GameUIController gameUI;
    private bool guides;

    private void Awake()
    {
        chassis = GetComponent<Chassis>();
        engine = GetComponent<Engine>();
        brakes = GetComponent<Brakes>();
        planeVisuals = GetComponent<PlaneVisuals>();

        rb = GetComponent<Rigidbody>();
        gameUI = FindObjectOfType<GameUIController>();
    }

    private void Start()
    {
        guides = PlayerPrefs.GetInt("Guides") == 1;
    }

    public void AssignInputs(Inputs inputs) { this.inputs = inputs; }

    // Метод задает начальные значения, таким образом можно
    // создавать самолет в воздухе уже летящимs
    public void SetStartValues(Vector3 velocity, float _thrust)
    {
        if (velocity != Vector3.zero) { chassis.ToggleChassis(); }

        rb.velocity = velocity;
        thrust = _thrust;
        engine.SetCurrentThrust(_thrust);
    }

    private void Update()
    {
        if (inputs != null)
        {
            if (inputs.ToggleChassis())
            {
                chassis.ToggleChassis();
            }

            if (inputs.Brakes())
            {
                brakes.Brake();
            }
            else
            {
                brakes.Unbrake();
            }

            planeVisuals.UpdateVisuals(yaw, pitch, roll, flaps);

            gameUI.UpdatePlaneInfo(thrust, (int)engine.rpm,
                (int)(magnitude * 3.6f * 0.53996f),
                (int)(transform.position.y * 3.28084f));

            sound.UpdateSounds(engine.rpm, transform.position.y * 3.28084f);

            float xAngle = Mathf.Abs(transform.eulerAngles.x > 180 ? transform.eulerAngles.x-90 - 360 : transform.eulerAngles.x-90);
            float zAngle = Mathf.Abs(transform.eulerAngles.z > 180 ? transform.eulerAngles.z - 360 : transform.eulerAngles.z);

            if (guides)
            {
                if (warnAboutBankAngle)
                    sound.BankAngle(zAngle > bankAngleZ);

                if (warnAboutPullUp)
                    sound.PullUp(transform.position.y * 3.28084f < pullUpHeight && xAngle < pullUpAngle);
            }
        }
        else
        {
            Debug.LogError("Inputs are not assigned");
        }
    }

    private void FixedUpdate()
    {
        magnitude = transform.InverseTransformDirection(rb.velocity).z;

        angle = Vector3.SignedAngle(transform.forward, rb.velocity, new Vector3(1, 0, 0));
        // Debug.Log(angle);

        // Сопротивление воздуха от крыльев под наклоном,
        // спустя время направление самолета будет выравниваться
        var mag = rb.velocity.magnitude;
        rb.velocity = mag * Vector3.Lerp(rb.velocity.normalized, transform.forward, Mathf.Abs(engine.SpeedAffect * angle) / 100);

        // Уменьшение вращательной инерции
        rb.angularVelocity *= 0.96f;

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

            AddFlapsLiftingForce();

            // Уменьшение скорости вперед из-за сопротивления воздуха
            rb.AddRelativeForce(-Vector3.forward * Mathf.Abs(engine.SpeedAffect * angle) * (chassis.IsClosed ? 10000 : 12000));
        }
    }

    private void AddYawForce()
    {
       rb.AddRelativeTorque(Vector3.up * yaw * magnitude * yawForce);
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
        rb.AddRelativeTorque(Vector3.right * flaps * magnitude * -pitchForce / 8);
        rb.AddRelativeForce(Vector3.up * flaps * magnitude * flapsForce);
    }

    public void OnTriggerEnter(Collider other)
    {
        Circle circle = other.GetComponent<Circle>();
        if (circle != null) { circle.Collected(); }

        if (other.CompareTag("Water") && engine.IsWorking)
        {
            engine.IsWorking = false;

            Debug.Log("Make lose when collide water"); // Stop game, disable camera

            sound.PlayWaterSplash();
        }
    }
}
