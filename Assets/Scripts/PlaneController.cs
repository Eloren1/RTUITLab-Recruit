using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Chassis))]
[RequireComponent(typeof(Engine))]
public class PlaneController : MonoBehaviour
{
    [Header("Параметры")]
    
    [Header("Детали")]
    private Chassis chassis;
    private Engine engine;

    [Header("Утилиты")]
    private Rigidbody rb;
    private Inputs inputs;

    private void Awake()
    {
        chassis = GetComponent<Chassis>();
        engine = GetComponent<Engine>();
    }

    public void AssignInputs(Inputs inputs) { this.inputs = inputs; }

    private void Update()
    {
        if (inputs != null)
        {
            if (inputs.ToggleChassis())
            {
                chassis.ToggleChassis();
            }


        } else
        {
            Debug.LogError("Inputs are not assigned");
        }
    }
}
