using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mov_PlayerShadow : MonoBehaviour
{
    [SerializeField] private Transform sourceTransform;
    [SerializeField] private float maxRaycastDistance;
    [SerializeField] private float scaleMultiplier = 0.1f;
    private Vector3 originalScale;
    private Vector3 minScale;

    void Start()
    {
        originalScale = transform.localScale;
        minScale = originalScale * scaleMultiplier;
    }

    void Update()
    {
        float distance = raycastFloorDistance();
        
        transform.position = sourceTransform.position - new Vector3(0, distance - 0.01f, 0);
        transform.localScale = Vector3.Lerp(originalScale, minScale, distance / maxRaycastDistance);
    }

    private float raycastFloorDistance() {
        Vector3 origin = sourceTransform.position;
        RaycastHit hit;
        Vector3 direction = -transform.up;

        int layerMask = LayerMask.GetMask("Default");

        if (Physics.Raycast(origin, direction, out hit, maxRaycastDistance, layerMask)) {
            float distance = (hit.point - origin).magnitude;
            return distance;
        }
        else {
            return maxRaycastDistance;
        }
    }
}