using UnityEngine;

public abstract class Inputs : MonoBehaviour
{
    private void Awake()
    {
        FindObjectOfType<PlaneController>().AssignInputs(this);
    }

    /// <returns>«начение от -1f до 1f</returns>
    public abstract float ThrustNormalized();

    /// <returns>«начение от -1f до 1f</returns>
    public abstract float RollNormalized();

    /// <returns>«начение от -1f до 1f</returns>
    public abstract float YawNormalized();

    /// <returns>«начение от -1f до 1f</returns>
    public abstract float PitchNormalized();

    public abstract bool ToggleChassis();

}
