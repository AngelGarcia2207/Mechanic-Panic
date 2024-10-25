using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int remainingLives = 10;
    [SerializeField] private int levelScore = 0;
    [SerializeField] private List<GameObject> players = new List<GameObject>();
    private bool isPaused = false;

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
        if (remainingLives > 0)
        {
            remainingLives -= 1;
            UI_Manager.Instance.UpdateRemainingLivesText(remainingLives);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void increaseLevelScore(int scoreToAdd)
    {
        levelScore += scoreToAdd;
        UI_Manager.Instance.UpdateScoreText(levelScore);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        UI_Manager.Instance.TogglePausePanel(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
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