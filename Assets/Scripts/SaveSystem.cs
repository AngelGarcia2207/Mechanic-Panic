using System;
using System.IO;
using UnityEngine;

public class SaveSystem
{
    private static readonly string savePath = Application.persistentDataPath + "/game.save";

    public static void SaveGame(GameData data)
    {
        Debug.Log("Guardando en: " + savePath);
        string json_data = JsonUtility.ToJson(data);
        string encryptedData = AESMethod.EncryptText(json_data);
        File.WriteAllText(savePath, encryptedData);
        PlayerPrefs.SetString("Date", DateTime.Now.ToString());
    }

    public static GameData LoadGame()
    {
        if (File.Exists(savePath))
        {
            string encryptedData = File.ReadAllText(savePath);
            string decryptedJson = AESMethod.DecryptText(encryptedData);
            return JsonUtility.FromJson<GameData>(decryptedJson);
        }
        else
        {
            Debug.LogError("No save file found!");
            return null;
        }
    }
}
