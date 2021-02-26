using UnityEngine;

public abstract class Inputs : MonoBehaviour
{
    private void Awake()
    {
        FindObjectOfType<PlaneController>().AssignInputs(this);
    }

    /// <returns>Значение от -1f до 1f</returns>
    public abstract float ThrustNormalized();

    /// <returns>Значение от -1f до 1f</returns>
    public abstract float YawNormalized();

    /// <returns>Значение от -1f до 1f</returns>
    public abstract float PitchNormalized();

    /// <returns>Значение от -1f до 1f</returns>
    public abstract float RollNormalized();

    /// <returns>Значение от -1f до 1f</returns>
    public abstract float FlapsNormalized();

    public abstract bool ToggleChassis();

}
