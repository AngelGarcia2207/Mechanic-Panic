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

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("score"))
        {
            PointsAmount.text = PlayerPrefs.GetInt("score").ToString();
        }
        else
        {
            PointsAmount.text = "0";
        }

        if (PlayerPrefs.HasKey("kills"))
        {
            RobotsAmount.text = PlayerPrefs.GetInt("kills").ToString();
        }
        else
        {
            RobotsAmount.text = "0";
        }

        if (PlayerPrefs.HasKey("deaths"))
        {
            DeathsAmount.text = PlayerPrefs.GetInt("deaths").ToString();
        }
        else
        {
            DeathsAmount.text = "0";
        }
    }

    public void Return()
    {
        SceneManager.LoadScene(ReturnMenu);
    }
}
