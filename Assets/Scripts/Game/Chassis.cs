using System.Collections;
using UnityEngine;

public class Chassis : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private float toggleDuration = 5.4f;

    private bool canToggle = true;
    public bool IsClosed = false;

    [SerializeField] private AudioSource sound;

    public void ToggleChassis()
    {
        if (canToggle)
        {
            canToggle = false;

            sound.Play();

            StartCoroutine(IsClosed ? Open() : Close());
        }
    }

    private IEnumerator Open()
    {
        animator.SetBool("Closed", false);

        yield return new WaitForSeconds(toggleDuration);

        IsClosed = false;
        canToggle = true;
    }

    private IEnumerator Close()
    {
        animator.SetBool("Closed", true);

        yield return new WaitForSeconds(toggleDuration);

        IsClosed = true;
        canToggle = true;
    }
}
