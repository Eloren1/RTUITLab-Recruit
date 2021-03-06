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

    public abstract bool Brakes();
}
