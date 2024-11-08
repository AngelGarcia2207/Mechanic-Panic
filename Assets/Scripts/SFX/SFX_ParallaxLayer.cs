using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_ParallaxLayer : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)] private float parallaxFactor;
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private Vector3 parallaxMovement;

    private void Start()
    {
        cameraTransform = GameObject.FindGameObjectWithTag("Camera").transform;

        lastCameraPosition = cameraTransform.position;
    }

    private void Update()
    {
        Vector3 cameraDelta = cameraTransform.position - lastCameraPosition;

        parallaxMovement = cameraDelta * parallaxFactor;

        transform.position += new Vector3(parallaxMovement.x, parallaxMovement.y, 0);

        lastCameraPosition = cameraTransform.position;
    }
}
