using UnityEngine;

public class KeyboardInputs : Inputs
{
    public override float ThrustNormalized()
    {
        return (Input.GetAxis("Thrust"));
    }

    public override float RollNormalized()
    {
        return (Input.GetAxis("Horizontal"));
    }

    public override float YawNormalized()
    {
        return (Input.GetAxis("Yaw"));
    }

    public override float PitchNormalized()
    {
        return (Input.GetAxis("Vertical"));
    }

    public override bool ToggleChassis()
    {
        return (Input.GetKeyDown(KeyCode.C));
    }
}
