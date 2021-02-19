using UnityEngine;

public abstract class Inputs : MonoBehaviour
{
    private void Awake()
    {
        FindObjectOfType<PlaneController>().AssignInputs(this);
    }

    /// <returns>�������� �� -1f �� 1f</returns>
    public abstract float ThrustNormalized();

    /// <returns>�������� �� -1f �� 1f</returns>
    public abstract float RollNormalized();

    /// <returns>�������� �� -1f �� 1f</returns>
    public abstract float YawNormalized();

    /// <returns>�������� �� -1f �� 1f</returns>
    public abstract float PitchNormalized();

    public abstract bool ToggleChassis();

}
