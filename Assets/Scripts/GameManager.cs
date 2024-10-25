using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int remainingLives = 10;
    [SerializeField] private int levelScore = 0;
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
}