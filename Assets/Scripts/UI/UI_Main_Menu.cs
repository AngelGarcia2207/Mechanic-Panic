using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Main_Menu : MonoBehaviour
{
    [SerializeField] private string Level1Scene;

    public void Play()
    {
        SceneManager.LoadScene(Level1Scene);
    }

    public void Config()
    {
        
    }

    public void Exit()
    {
        Application.Quit();
    }
}
