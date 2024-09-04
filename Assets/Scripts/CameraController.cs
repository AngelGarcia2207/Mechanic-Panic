using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 distance;
    [SerializeField] private Vector3 angle;

    void Update()
    {
        transform.position = target.position + distance;

        Quaternion rotation = Quaternion.Euler(angle);
        transform.rotation = rotation;
    }
}
