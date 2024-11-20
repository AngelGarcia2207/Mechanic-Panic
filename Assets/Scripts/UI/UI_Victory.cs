using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UI_Victory : MonoBehaviour
{
    [SerializeField] private string ReturnMenu;
    [SerializeField] private TMP_Text PointsAmount;
    [SerializeField] private TMP_Text RobotsAmount;
    [SerializeField] private TMP_Text DeathsAmount;

    public UI_Manager UIManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

   public void Return()
    {
        SceneManager.LoadScene(ReturnMenu);
    }

   public void UpdatePoints(int score)
    {

    }
}
