using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_Default_Controller : MonoBehaviour
{
    [SerializeField] private Vector3 distance;
    [SerializeField] private Vector3 angle;
    private new Camera camera;
    private Transform target;

    void Start()
    {   
        camera = GetComponent<Camera>();

        target = GameObject.FindWithTag("CameraCenter").transform;
        
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        transform.position = target.position + distance;

        Quaternion rotation = Quaternion.Euler(angle);
        transform.rotation = rotation;
    }

    public void UpdateOrthographicSize(float size)
    {
        camera.orthographicSize = size;
        Map_Display_Boundaries.Instance.UpdateBoundaries();
    }

    public Vector2 CalculateOrthographicPlaneSize() {
        float height = 2f * camera.orthographicSize;
        float width = height * camera.aspect;

        return new Vector2(width, height*1.5f);
    }
}