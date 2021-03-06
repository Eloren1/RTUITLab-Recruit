using System.Collections;
using UnityEngine;

public abstract class Inputs : MonoBehaviour
{
    private PlaneController controller;

    private void Awake()
    {
        StartCoroutine(TryToAssign());
    }

    private IEnumerator TryToAssign()
    {
        controller = FindObjectOfType<PlaneController>();

        while (controller == null)
        {
            yield return new WaitForSeconds(0.2f);
            controller = FindObjectOfType<PlaneController>();
        }

        controller.AssignInputs(this);
    }

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
