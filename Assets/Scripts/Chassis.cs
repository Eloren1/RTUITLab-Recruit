using System.Collections;
using UnityEngine;

public class Chassis : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private float toggleDuration = 5.4f;

    private bool canToggle = true;
    private bool isClosed = false;

    public void ToggleChassis()
    {
        if (canToggle)
        {
            canToggle = false;
            StartCoroutine(isClosed ? Open() : Close());
        }
    }

    private IEnumerator Open()
    {
        animator.SetBool("Closed", false);

        yield return new WaitForSeconds(toggleDuration);

        isClosed = false;
        canToggle = true;
    }

    private IEnumerator Close()
    {
        animator.SetBool("Closed", true);

        yield return new WaitForSeconds(toggleDuration);

        isClosed = true;
        canToggle = true;
    }
}
