using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using Unity.Netcode;

public class UI_PlayerCard_Manager : MonoBehaviour
{
    [SerializeField] private GameObject PlayerCardPrefabLocal, PlayerCardPrefabOnline;

    public static UI_PlayerCard_Manager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    public GameObject CreatePlayerCard(GameObject newCard, Sprite headSprite, string name)
    {
        GameObject chosenPlayerCardPrefab = (GetComponent<NetworkObject>() == null) ? PlayerCardPrefabLocal : PlayerCardPrefabOnline;

        if (newCard == null)
        { newCard = Instantiate(chosenPlayerCardPrefab, transform.position, transform.rotation, transform); }

        Image newCardImage = newCard.transform.Find("HeadImage").GetComponentInChildren<Image>();
        TMP_Text newCardText = newCard.GetComponentInChildren<TMP_Text>();


        newCardImage.sprite = headSprite;
        newCardText.text = name;

        return newCard;
    }
}
