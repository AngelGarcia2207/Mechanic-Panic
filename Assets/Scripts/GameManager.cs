using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int remainingLives = 10;
    [SerializeField] private int levelScore = 0;
    [SerializeField] private List<GameObject> players = new List<GameObject>();
    private bool isPaused = false;
    private bool isShowingConfig = false;

    // ONLINE
    [HideInInspector] public Onl_Player_Manager onlManager;
    private bool isOnline = false;

    public static GameManager Instance { get; private set; }

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
        onlManager = GameObject.FindFirstObjectByType<Onl_Player_Manager>();
        if(onlManager != null)
        {
            isOnline = true;
        }
    }

    public void AddPlayer(GameObject newPlayer)
    {
        if (players.Count < 4)
        {
            players.Add(newPlayer);
        }
    }

    public void checkForAlivePlayers()
    {
        bool atLeastOnePlayerAlive = false;
        foreach (GameObject player in players)
        {
            Mov_Player_Controller playerScript = player.GetComponent<Mov_Player_Controller>();
            if (playerScript.GetAliveStatus() == true)
            {
                atLeastOnePlayerAlive = true;
            }
        }

        if (!atLeastOnePlayerAlive && remainingLives < 1)
        {
            UI_Manager.Instance.ShowGameOverPanel();
        }
    }

    public int GetRemainingLives()
    {
        return remainingLives;
    }

    public bool ConsumeALive()
    {
        if (GetRemainingLives(0, "Add") > 0)
        {
            //remainingLives -= 1;
            GetRemainingLives(-1, "Add");
            UI_Manager.Instance.UpdateRemainingLivesText(remainingLives);
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetRemainingLives(int value, string setOrAdd)
    {
        if (setOrAdd == "Set") { remainingLives = value; }
        else if (setOrAdd == "Add") { remainingLives += value; }
        if (isOnline)
        {
            onlManager.TryRemLivesOnline(remainingLives);
            return onlManager.remainingLivesOnl;
        }
        return remainingLives;
    }

    public void increaseLevelScore(int scoreToAdd)
    {
        levelScore += scoreToAdd;
        UI_Manager.Instance.UpdateScoreText(levelScore);
    }

    public void TogglePause()
    {
        if (isShowingConfig)
        {
            UI_Manager.Instance.HideConfigPanel();
        }
        else if (isPaused)
        {
            isPaused = false;
            UI_Manager.Instance.TogglePausePanel(isPaused);
            Time.timeScale = 1f;
        }
        else
        {
            isPaused = true;
            UI_Manager.Instance.TogglePausePanel(isPaused);
            Time.timeScale = 0f;
        }
    }

    public void ToggleConfig(bool status)
    {
        isShowingConfig = status;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }
}