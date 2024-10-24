using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_Default_Controller : MonoBehaviour
{
    [SerializeField] private Vector3 distance;
    [SerializeField] private Vector3 angle;
    [SerializeField] [Range(0f, 1f)] private float LerpValue;
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
        UpdatePosition();

        Quaternion rotation = Quaternion.Euler(angle);
        transform.rotation = rotation;
    }

    public void UpdatePosition()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + distance, LerpValue);
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