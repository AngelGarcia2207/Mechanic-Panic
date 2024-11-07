using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    [SerializeField] private Transform sourceTransform;
    [SerializeField] private float maxRaycastDistance;

    [SerializeField] private float scaleMultiplier = 0.1f;
    private Vector3 originalScale;
    private float originalDistance;

    void Start()
    {
        originalScale = transform.localScale;
        originalDistance = (transform.position - sourceTransform.position).magnitude;
    }
    

    void Update()
    {
        float distance = raycastFloorDistance();
        
        transform.position = sourceTransform.position - new Vector3(0, distance, 0);
        // transform.localScale =
    }

    private float raycastFloorDistance() {
        Vector3 origin = sourceTransform.position;
        RaycastHit hit;
        Vector3 direction = -transform.up;

        if (Physics.Raycast(origin, direction, out hit, maxRaycastDistance)) {
            float distance = (hit.point - origin).magnitude;
            return distance;
        }
        else {
            return float.MaxValue;
        }
    }
}
