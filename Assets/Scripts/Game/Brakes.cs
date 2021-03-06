using UnityEngine;

public class Brakes : MonoBehaviour
{
    [SerializeField] private Collider[] wheels;
    [SerializeField] private PhysicMaterial wheelRotating;
    [SerializeField] private PhysicMaterial wheelBraking;

    public void Brake()
    {
        foreach (var wheel in wheels)
        {
            wheel.material = wheelBraking;
        }
    }

    public void Unbrake()
    {
        foreach (var wheel in wheels)
        {
            wheel.material = wheelRotating;
        }
    }
}
