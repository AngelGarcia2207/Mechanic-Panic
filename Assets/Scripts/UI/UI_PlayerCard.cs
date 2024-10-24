using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_PlayerCard : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;
    [SerializeField] private GameObject DeadPanel;
    
    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        healthBarImage.fillAmount = (float) currentHealth / maxHealth;
    }

    public void ToggleDeadPanel()
    {
        if (DeadPanel.activeSelf)
        {
            DeadPanel.SetActive(false);
        }
        else
        {
            DeadPanel.SetActive(true);
        }
    }
}