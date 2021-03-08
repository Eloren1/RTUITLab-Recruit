using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlaneVisuals))]
[RequireComponent(typeof(Chassis))]
[RequireComponent(typeof(Engine))]
[RequireComponent(typeof(Brakes))]
public class PlaneController : MonoBehaviour
{
    [Header("���������")]
    [SerializeField] private float yawForce = 2500f;
    [SerializeField] private float pitchForce = 3200f;
    [SerializeField] private float rollForce = 7000f;
    [SerializeField] private float flapsForce = 5000f;
    [SerializeField] private float changingSpeed = 0.1f;

    [Header("����������")]
    private float thrust;
    private float yaw;
    private float pitch;
    private float roll;
    private float flaps;
    private float magnitude;
    private float angle;
    
    [Header("����������")]
    private Chassis chassis;
    private Engine engine;
    private Brakes brakes;
    private PlaneVisuals planeVisuals;

    [Header("�������")]
    [SerializeField] private PlaneSound sound;
    private Rigidbody rb;
    private Inputs inputs;
    private GameUIController gameUI;

    private void Awake()
    {
        chassis = GetComponent<Chassis>();
        engine = GetComponent<Engine>();
        brakes = GetComponent<Brakes>();
        planeVisuals = GetComponent<PlaneVisuals>();

        rb = GetComponent<Rigidbody>();
        gameUI = FindObjectOfType<GameUIController>();
    }

    public void AssignInputs(Inputs inputs) { this.inputs = inputs; }

    // ����� ������ ��������� ��������, ����� ������� �����
    // ��������� ������� � ������� ��� �������
    public void SetStartValues(Vector3 velocity)
    {
        rb.velocity = velocity;
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
            } else
            {
                brakes.Unbrake();
            }

            planeVisuals.UpdateVisuals(yaw, pitch, roll, flaps);

            gameUI.UpdatePlaneInfo(thrust, (int)engine.rpm, 
                (int)(magnitude * 3.6f * 0.53996f), 
                (int)(transform.position.y * 3.28084f));

            sound.UpdateSounds(engine.rpm, transform.position.y * 3.28084f);
        } else
        {
            Debug.LogError("Inputs are not assigned");
        }
    }

    private void FixedUpdate()
    {
        magnitude = transform.InverseTransformDirection(rb.velocity).z;

        angle = Vector3.SignedAngle(transform.forward, rb.velocity, new Vector3(1, 0, 0));
        // Debug.Log(angle);

        // ������������� ������� �� ������� ��� ��������,
        // ������ ����� ����������� �������� ����� �������������
        var mag = rb.velocity.magnitude;
        rb.velocity = mag * Vector3.Lerp(rb.velocity.normalized, transform.forward, Mathf.Abs(engine.SpeedAffect * angle) / 100);

        // ���������� ������������ �������
        rb.angularVelocity *= 0.96f;

        if (inputs != null)
        {
            // ��� ���������� ����� �������� �� ��������
            thrust = Mathf.Clamp(thrust + inputs.ThrustNormalized() * 0.01f, 0f, 1f);
            flaps = Mathf.Clamp(flaps + inputs.FlapsNormalized() * 0.02f, 0f, 1f);

            // ��� ���������� ����� �������� ������������ � ����
            yaw = Mathf.Lerp(yaw, inputs.YawNormalized(), changingSpeed);
            pitch = Mathf.Lerp(pitch, inputs.PitchNormalized(), changingSpeed);
            roll = Mathf.Lerp(roll, inputs.RollNormalized(), changingSpeed);


            engine.AddForce(thrust, magnitude);

            AddYawForce();
            AddRollForce();
            AddPitchForce();

            AddFlapsLiftingForce();

            // ���������� �������� ������ ��-�� ������������� �������
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
        rb.AddRelativeTorque(Vector3.right * flaps * magnitude * -pitchForce / 5);
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