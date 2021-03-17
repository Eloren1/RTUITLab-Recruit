using System.Collections;
using UnityEngine;

public abstract class Inputs : MonoBehaviour
{
    private PlaneController controller;

    /// <returns>�������� �� -1f �� 1f</returns>
    public abstract float ThrustNormalized();

    /// <returns>�������� �� -1f �� 1f</returns>
    public abstract float YawNormalized();

    /// <returns>�������� �� -1f �� 1f</returns>
    public abstract float PitchNormalized();

    /// <returns>�������� �� -1f �� 1f</returns>
    public abstract float RollNormalized();

    /// <returns>�������� �� -1f �� 1f</returns>
    public abstract float FlapsNormalized();

    public abstract bool ToggleChassis();

    public abstract bool Brakes();
}
