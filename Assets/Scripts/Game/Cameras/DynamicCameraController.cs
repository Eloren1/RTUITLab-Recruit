using UnityEngine;

public class DynamicCameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    private float distance;
    [SerializeField] private float maxDistance = 50;
    [SerializeField] private Vector3 startOffset = new Vector3(-30, 20, 20);

    private void OnEnable()
    {
        transform.position = target.position + startOffset;
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            distance = Vector3.Distance(transform.position, target.position);

            if (distance > maxDistance)
            {
                ChangePosition();
            }

            transform.LookAt(target.position + target.forward * 5);
        }
    }

    private void ChangePosition()
    {
        Vector3 difference = target.position - transform.position;
        difference.y = 0;
        difference /= 1.2f;

        transform.position = target.position + difference + Vector3.up * 10;
    }
}
