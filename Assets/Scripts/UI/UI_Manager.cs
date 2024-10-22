using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private TMP_Text remainingLivesAmount;

    public static UI_Manager Instance { get; private set; }

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
        remainingLivesAmount.text = GameManager.Instance.GetRemainingLives().ToString();
    }

    public void UpdateRemainingLivesText(int remainingLives)
    {
        remainingLivesAmount.text = remainingLives.ToString();
    }
}
