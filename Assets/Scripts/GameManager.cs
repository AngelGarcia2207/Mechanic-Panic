using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int remainingLives = 10;
    [SerializeField] private int levelScore;

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
}