using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro; 
using UnityEngine;

public class SaveNumber : MonoBehaviour
{
    public TMP_InputField numberInput;
    public void SavingLevel()
    {
        Debug.Log("numberInput: " + numberInput);
        GameData data = new GameData();
        if (int.TryParse(numberInput.text, out int number))
        {
            data.levelProgress = number;
            SaveSystem.SaveGame(data);
            Debug.Log("Número guardado: " + number);
        }
        else
        {
            Debug.LogError("Entrada no válida. Asegúrate de ingresar un número.");
        }
    }
    public void LoadLevel()
    {
        GameData data = SaveSystem.LoadGame();
        if (data != null)
        {
            numberInput.text = data.levelProgress.ToString();
            Debug.Log("Número cargado: " + data.levelProgress);
        }
        else
        {
            Debug.LogError("No se pudo cargar ningún número, archivo no encontrado o vacío.");
        }
    }
}
