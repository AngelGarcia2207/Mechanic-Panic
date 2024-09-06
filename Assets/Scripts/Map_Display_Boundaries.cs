using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Display_Boundaries : MonoBehaviour
{
    [SerializeField] private float backBoundaryDistance;
    [SerializeField] private float frontBoundaryDistance;
    [SerializeField] private float topHeightDistance;

    private GameObject cameraObject;
    private Cam_Default_Controller cameraScript;

    private GameObject player;

    private GameObject leftBoundary;
    private GameObject rightBoundary;
    private GameObject topBoundary;
    private GameObject backBoundary;
    private GameObject frontBoundary;

    void Start()
    {
        cameraObject = GameObject.FindWithTag("Camera");
        cameraScript = cameraObject.GetComponent<Cam_Default_Controller>();

        player = GameObject.FindWithTag("Player"); // Temporalmente solo apuntaré a un jugador, después serán varios
        Debug.Log(player);

        leftBoundary = new GameObject("leftBoundary");
        rightBoundary = new GameObject("rightBoundary");
        topBoundary = new GameObject("topBoundary");
        backBoundary = new GameObject("backBoundary");
        frontBoundary = new GameObject("frontBoundary");

        leftBoundary.transform.parent = this.transform;
        rightBoundary.transform.parent = this.transform;
        topBoundary.transform.parent = this.transform;
        backBoundary.transform.parent = this.transform;
        frontBoundary.transform.parent = this.transform;

        AddBoxCollider(leftBoundary);
        AddBoxCollider(rightBoundary);
        AddBoxCollider(topBoundary);
        AddBoxCollider(backBoundary);
        AddBoxCollider(frontBoundary);

        // Descomenta estas lineas si quieres que se vean las cajas
        // AddMeshComponents(leftBoundary);
        // AddMeshComponents(rightBoundary);
        // AddMeshComponents(topBoundary);
        // AddMeshComponents(backBoundary);
        // AddMeshComponents(frontBoundary);
    }

    void Update()
    {
        followPlayers();

        updateBoundaries(); // Temporalmente en el Update, después se llamará a través del patrón Observer
    }

    private void followPlayers() {
        transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
    }

    private void AddBoxCollider(GameObject obj)
    {
        BoxCollider boxCollider = obj.AddComponent<BoxCollider>();
        boxCollider.size = Vector3.one;
        obj.layer = LayerMask.NameToLayer("PlayerBoundary");
    }

    private void AddMeshComponents(GameObject obj)
    {
        MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();

        meshFilter.mesh = CreateCubeMesh();

        meshRenderer.material = new Material(Shader.Find("Standard"));  
    }

    private Mesh CreateCubeMesh()
    {
        Mesh mesh = new Mesh();

        mesh.vertices = new Vector3[]
        {
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, 0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f)
        };

        mesh.triangles = new int[]
        {
            0, 2, 1, 0, 3, 2,
            4, 5, 6, 4, 6, 7,
            0, 1, 5, 0, 5, 4,
            2, 3, 7, 2, 7, 6,
            0, 4, 7, 0, 7, 3,
            1, 2, 6, 1, 6, 5
        };

        mesh.uv = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1),
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1)
        };

        mesh.RecalculateNormals();
        return mesh;
    }

    public void updateBoundaries()
    {
        Vector2 targetPlaneSize = cameraScript.CalculateTargetPlaneSize();

        Vector3 backFrontScale = new Vector3(targetPlaneSize.x * 2, topHeightDistance, 1);
        Vector3 leftRightScale = new Vector3(1, topHeightDistance, targetPlaneSize.y);
        Vector3 topScale = new Vector3(targetPlaneSize.x * 2, topHeightDistance, targetPlaneSize.y);

        backBoundary.transform.position = transform.position + new Vector3(0, 0, backBoundaryDistance);
        backBoundary.transform.localScale = backFrontScale;

        frontBoundary.transform.position = transform.position + new Vector3(0, 0, -frontBoundaryDistance);
        frontBoundary.transform.localScale = backFrontScale;
        
        leftBoundary.transform.position = transform.position + new Vector3(-targetPlaneSize.x, 0, 0);
        leftBoundary.transform.localScale = leftRightScale;

        rightBoundary.transform.position = transform.position + new Vector3(targetPlaneSize.x, 0, 0);
        rightBoundary.transform.localScale = leftRightScale;

        topBoundary.transform.position = transform.position + new Vector3(0, topHeightDistance, 0);
        topBoundary.transform.localScale = topScale;
    }
}
