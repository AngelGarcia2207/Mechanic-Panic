using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_Default_Controller : MonoBehaviour
{
    [SerializeField] private Vector3 distance;
    [SerializeField] private Vector3 angle;
    private new Camera camera;
    private Transform target;
    enum LockCursor { Locked, Unlocked }
    [SerializeField] private LockCursor cursorLock;

    void Start()
    {   
        camera = GetComponent<Camera>();

        target = GameObject.FindWithTag("CameraCenter").transform;

        if (cursorLock == LockCursor.Locked)
        { Cursor.lockState = CursorLockMode.Locked; }
    }

    void Update()
    {
        transform.position = target.position + distance;

        Quaternion rotation = Quaternion.Euler(angle);
        transform.rotation = rotation;
    }

    public Vector2 CalculateNearPlaneSize() {
        float height = Mathf.Tan(camera.fieldOfView * Mathf.Deg2Rad / 2) * camera.nearClipPlane;
        float width = height * camera.aspect;

        return new Vector2(width, height);
    }

    public Vector2 CalculateTargetPlaneSize() {
        Vector2 nearPlaneSize = CalculateNearPlaneSize(); 

        float targetDistance = (target.position - transform.position).magnitude;
        float height = targetDistance * nearPlaneSize.y / camera.nearClipPlane;
        float width = height * camera.aspect;

        return new Vector2(width, height);
    }
}
