using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Main_Menu : MonoBehaviour
{
    [SerializeField] private string Level1Scene;
    [SerializeField] private GameObject configPanel;
    [SerializeField] private CanvasGroup buttonPlay;
    [SerializeField] private CanvasGroup buttonExit;
    [SerializeField] private CanvasGroup buttonConfig;

    public void Play()
    {
        SceneManager.LoadScene(Level1Scene);
    }

    public void Config()
    {
        // Activa el panel de configuración
        configPanel.SetActive(true);

        //Desactivar otros botones
        buttonPlay.interactable = false;
        buttonPlay.alpha = 0;

        buttonExit.interactable = false;
        buttonExit.alpha = 0;

        buttonConfig.interactable = false;
        buttonConfig.alpha = 0;
    }

    public void CloseConfig()
    {
        // Activa el panel de configuración
        configPanel.SetActive(false);

        //Desactivar otros botones
        buttonPlay.interactable = true;
        buttonPlay.alpha = 1;

        buttonExit.interactable = true;
        buttonExit.alpha = 1;

        buttonConfig.interactable = true;
        buttonConfig.alpha = 1;
    }

    public void Exit()
    {
        Application.Quit();
        Debug.Log("Saliste del Juego");
    }
}
