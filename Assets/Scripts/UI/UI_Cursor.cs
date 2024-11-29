using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Cursor : MonoBehaviour
{
    public float speed;
    [HideInInspector] public Mov_Player_Controller playerController;
    [HideInInspector] public UI_SelectionCard hoveredCard, selectedCard;
    [HideInInspector] public UI_Play_Selection hoveredPlayButton;
    private int playerID;

    void Start()
    {
        transform.SetParent(GameObject.FindGameObjectWithTag("CharacterSelection").transform);
    }

    public void ChangeCursorColor(int _playerID)
    {
        playerID = _playerID;

        Color chosenColor = new Color(0, 0, 0);
        if (playerID == 0)
        {chosenColor = new Color(0.9717f, 0.3896f, 0.3896f); }
        else if (playerID == 1)
        { chosenColor = new Color(0.31372f, 0.68205f, 0.88679f); }
        else if (playerID == 2)
        { chosenColor = new Color(1f, 0.96035f, 0.51415f); }
        else if (playerID == 3)
        { chosenColor = new Color(0.6362f, 0.9434f, 0.44055f); }

        GetComponent<Image>().color = chosenColor;
    }

    void FixedUpdate()
    {
        if(playerController != null && playerController.isOnline)
        {
            Click();
        }
    }

    public void Click()
    {
        if (hoveredPlayButton != null)
        {
            hoveredPlayButton.Play();
        }
        else if (hoveredCard != null)
        {
            selectedCard = hoveredCard.GetSelectedLocal(playerID, this, selectedCard);
        }
    }

    public void BackClick()
    {
        if (selectedCard != null)
        {
            selectedCard.Deselect();
            selectedCard = null;
        }
    }
}
