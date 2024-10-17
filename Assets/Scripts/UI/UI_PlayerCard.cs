using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_PlayerCard : MonoBehaviour
{
    [SerializeField] private GameObject PlayerCardPrefab;
    [SerializeField] private GameObject PlayersCards;

    public static UI_PlayerCard Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    public GameObject CreatePlayerCard(Sprite headSprite, string name)
    {
        GameObject newCard = Instantiate(PlayerCardPrefab, transform.position, transform.rotation, transform);

        Image newCardImage = newCard.transform.Find("HeadImage").GetComponentInChildren<Image>();
        TMP_Text newCardText = newCard.GetComponentInChildren<TMP_Text>();


        newCardImage.sprite = headSprite;
        newCardText.text = name;

        return newCard;
    }
}