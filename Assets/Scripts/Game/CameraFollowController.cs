using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowController : MonoBehaviour
{ 
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    private void Update()
    {
        if (target != null)
        {
            transform.position = target.position + target.TransformDirection(offset);
            transform.LookAt(target.position);
        }
    }
}