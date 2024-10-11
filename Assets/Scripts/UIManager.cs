using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image healthBarImage;

    public static UIManager Instance { get; private set; }

    void Awake() // Cambia Start a Awake
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        healthBarImage.fillAmount = (float) currentHealth / maxHealth;
    }
}
