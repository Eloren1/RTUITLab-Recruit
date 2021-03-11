using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowController : MonoBehaviour
{ 
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    private void OnEnable()
    {
        FollowTarget();
    }

    private void Update()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        if (target != null)
        {
            transform.position = target.position + target.TransformDirection(offset);
            transform.LookAt(target.position);
        }
    }
}