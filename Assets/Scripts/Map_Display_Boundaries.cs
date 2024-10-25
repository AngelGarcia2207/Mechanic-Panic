using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Display_Boundaries : MonoBehaviour
{
    [SerializeField] private float backFrontBoundaryDistance;
    [SerializeField] private float topHeightDistance;
    [SerializeField] private GameObject boundaryPrefab;
    private GameObject cameraObject;
    private Cam_Default_Controller cameraScript;

    [SerializeField] private List<GameObject> players = new List<GameObject>();

    private GameObject leftBoundary;
    private GameObject rightBoundary;
    private GameObject topBoundary;
    private GameObject backBoundary;
    private GameObject frontBoundary;

    public static Map_Display_Boundaries Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        cameraObject = GameObject.FindWithTag("Camera");
        cameraScript = cameraObject.GetComponent<Cam_Default_Controller>();

        leftBoundary = Instantiate(boundaryPrefab, transform.position, transform.rotation, transform);
        rightBoundary = Instantiate(boundaryPrefab, transform.position, transform.rotation, transform);
        topBoundary = Instantiate(boundaryPrefab, transform.position, transform.rotation, transform);
        backBoundary = Instantiate(boundaryPrefab, transform.position, transform.rotation, transform);
        frontBoundary = Instantiate(boundaryPrefab, transform.position, transform.rotation, transform);
    }

    void Update()
    {
        if (players.Count > 0)
        {
            followPlayers();
            UpdateBoundaries(); // Temporalmente en el Update, después se llamará a través del patrón Observer
        }
    }

    private void followPlayers()
    {
        if (players.Count > 0)
        {
            Vector3 averagePosition = Vector3.zero;

            foreach (GameObject player in players)
            {
                averagePosition += player.transform.position;
            }

            averagePosition /= players.Count;

            transform.position = new Vector3(averagePosition.x, transform.position.y, transform.position.z);
        }
    }

    public void AddPlayer(GameObject newPlayer)
    {
        if (players.Count < 4)
        {
            players.Add(newPlayer);
        }
    }

    public void RemovePlayer(GameObject playerToRemove)
    {
        if (players.Count > 0)
        {
            players.Remove(playerToRemove);
        }
    }

    public void UpdateBoundaries() {
        Vector2 orthographicPlaneSize = cameraScript.CalculateOrthographicPlaneSize();

        Vector3 backFrontScale = new Vector3(orthographicPlaneSize.x, orthographicPlaneSize.y, 1);
        Vector3 leftRightScale = new Vector3(1, orthographicPlaneSize.y, backFrontBoundaryDistance);
        Vector3 topScale = new Vector3(orthographicPlaneSize.x, 1, backFrontBoundaryDistance);

        backBoundary.transform.position = transform.position + new Vector3(0, 0, backFrontBoundaryDistance / 2);
        backBoundary.transform.localScale = backFrontScale;

        frontBoundary.transform.position = transform.position + new Vector3(0, 0, -backFrontBoundaryDistance / 2);
        frontBoundary.transform.localScale = backFrontScale;

        leftBoundary.transform.position = transform.position + new Vector3(-orthographicPlaneSize.x / 2, 0, 0);
        leftBoundary.transform.localScale = leftRightScale;

        rightBoundary.transform.position = transform.position + new Vector3(orthographicPlaneSize.x / 2, 0, 0);
        rightBoundary.transform.localScale = leftRightScale;

        topBoundary.transform.position = transform.position + new Vector3(0, orthographicPlaneSize.y / 2, 0);
        topBoundary.transform.localScale = topScale;
    }
}