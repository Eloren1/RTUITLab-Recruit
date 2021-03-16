using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlaneVisuals))]
[RequireComponent(typeof(Chassis))]
[RequireComponent(typeof(Engine))]
[RequireComponent(typeof(Brakes))]
public class PlaneController : MonoBehaviour
{
    [SerializeField] private float gravityScale = 1.0f;
    public static float globalGravity = -14.1264f;

    [Header("Параметры")]
    [SerializeField] private float yawForce = 2500f;
    [SerializeField] private float yawVisualForce = 8f;
    [SerializeField] private float yawStoppingForce = 30000f;
    [SerializeField] private float pitchForce = 3200f;
    [SerializeField] private float rollForce = 7000f;
    [SerializeField] private float flapsForce = 25000f;
    [Tooltip("Какая часть силы Flaps будет постоянно добавляться как подъемная")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float flapsForceToLiftingForce = 0.25f;
    [SerializeField] private float changingSpeed = 0.1f;

    [SerializeField] private float minSpeedAlarm = 50f;
    [SerializeField] private float maxSpeedAlarm = 220f;
    [SerializeField] private float maxSpeedBeforeLose = 280f;

    [SerializeField] private bool warnAboutBankAngle = false;
    [SerializeField] private float bankAngleZ = 75f;
    [SerializeField] private bool warnAboutPullUp = true;
    [SerializeField] private float pullUpAngle = 55f;
    [Tooltip("Высота в футах")]
    [SerializeField] private float pullUpHeight = 4000f;

    [Header("Управление")]
    private float thrust;
    private float yaw;
    private Vector3 currentYawAngles;
    private float pitch;
    private float roll;
    private float flaps;
    private float magnitude;
    public float Magnitude { get { return magnitude; } }
    public float SpeedInKnots;
    private float angle;
    
    [Header("Компоненты")]
    [SerializeField] private GameObject planeModel;
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
    private bool isGameActive = true;

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
        if (!isGameActive) return;

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

            if (SpeedInKnots > maxSpeedBeforeLose)
            {
                StopGame("СЛИШКОМ БОЛЬШАЯ СКОРОСТЬ САМОЛЕТА");
            }

            planeVisuals.UpdateVisuals(yaw, pitch, roll, flaps);

            gameUI.UpdatePlaneInfo(thrust, (int)engine.rpm,
                (int)(SpeedInKnots),
                (int)(transform.position.y * 3.28084f));

            sound.UpdateSounds(engine.rpm, transform.position.y * 3.28084f);

            WarningSounds();
        }
        else
        {
            Debug.LogError("Inputs are not assigned");
        }
    }

    private void WarningSounds()
    {
        float xAngle = Mathf.Abs(transform.eulerAngles.x > 180 ? transform.eulerAngles.x - 90 - 360 : transform.eulerAngles.x - 90);
        float zAngle = Mathf.Abs(transform.eulerAngles.z > 180 ? transform.eulerAngles.z - 360 : transform.eulerAngles.z);

        if (guides)
        {
            if (warnAboutBankAngle)
                sound.BankAngle(zAngle > bankAngleZ);

            if (warnAboutPullUp)
                sound.PullUp(transform.position.y * 3.28084f < pullUpHeight && xAngle < pullUpAngle);
        }

        sound.CheckSpeed(SpeedInKnots > maxSpeedAlarm || 
                        (SpeedInKnots < minSpeedAlarm && transform.position.y * 3.28084f > 100f));
    }

    private void AddCustomGravity()
    {
        rb.AddForce(Vector3.up * globalGravity * gravityScale * Mathf.Clamp(1.6f - (SpeedInKnots / 120), 0.1f, 1000f), ForceMode.Acceleration);
    }

    private void FixedUpdate()
    {
        if (!isGameActive) return;

        SpeedInKnots = magnitude * 3.6f * 0.53996f;
        magnitude = transform.InverseTransformDirection(rb.velocity).z;
        angle = Vector3.SignedAngle(transform.forward, rb.velocity, new Vector3(1, 0, 0));

        // Debug.Log(rb.velocity); // Нужно для получения стартовых значений при создании в воздухе

        AddCustomGravity();

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

            engine.AddForce(thrust, magnitude, SpeedInKnots);

            AddYawForce();
            AddRollForce();
            AddPitchForce();

            AddFlapsLiftingForce();
            AddWingLiftingForce();
        }
    }

    private void AddYawForce()
    {
        rb.AddRelativeTorque(Vector3.up * yaw * magnitude * yawForce);

        currentYawAngles = planeModel.transform.localEulerAngles;
        if (currentYawAngles.y > 180f) currentYawAngles.y -= 360f;
        planeModel.transform.localEulerAngles = Vector3.Lerp(currentYawAngles, Vector3.up * yaw * magnitude / 50 * yawVisualForce, 0.1f);

        rb.AddRelativeForce(Vector3.forward * -yawStoppingForce * Mathf.Abs(yaw) * magnitude);
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
        rb.AddRelativeTorque(Vector3.right * flaps * magnitude * -pitchForce / 14);
        rb.AddRelativeForce(Vector3.up * flaps * magnitude * flapsForce);
    }

    private void AddWingLiftingForce()
    {
        rb.AddRelativeTorque(Vector3.right * magnitude * -pitchForce / 14 * flapsForceToLiftingForce);
        rb.AddRelativeForce(Vector3.up * magnitude * flapsForce * flapsForceToLiftingForce);
    }

    public void OnTriggerEnter(Collider other)
    {
        // Пролет через кольцо в соренованиях
        Circle circle = other.GetComponent<Circle>();
        if (circle != null)
        {
            sound.PlayCollectedSound();
            circle.Collected();
        }

        // Падение самолета в воду
        if (other.CompareTag("Water") && engine.IsWorking)
        {
            StopGame("САМОЛЕТ СТОЛКНУЛСЯ С ВОДОЙ");

            sound.PlayWaterSplash();
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (magnitude > 20f)
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 localCollisionPoint = transform.InverseTransformPoint(contact.point);

            // Debug.Log(localCollisionPoint);

            bool rearChassisTouch = (localCollisionPoint.y < -0.7f && localCollisionPoint.z < -2.2f);
            bool frontChassisTouch = (localCollisionPoint.y < -1.6f);

            if (rearChassisTouch || frontChassisTouch)
            {
                // Самолет коснулся при помощи шасси
            } else
            {
                StopGame("САМОЛЕТ СТОЛКНУЛСЯ С ЗЕМЛЕЙ");
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        // Вылет самолета за границы
        if (other.CompareTag("Map Edge"))
        {
            StopGame("ВЫ ВЫЛЕТЕЛИ ЗА ГРАНИЦЫ КАРТЫ");
        }
    }

    public void StopGame(string reason)
    {
        if (isGameActive)
        {
            StopGame();

            gameUI.ShowEndingScreen(reason, "", false);
        }
    }

    public void StopGame()
    {
        if (isGameActive)
        {
            sound.gameObject.SetActive(false);

            isGameActive = false;
            engine.IsWorking = false;
        }
    }
}
