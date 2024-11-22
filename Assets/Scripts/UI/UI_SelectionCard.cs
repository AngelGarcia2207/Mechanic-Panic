using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SelectionCard : MonoBehaviour
{
    [SerializeField] private int characterID;
    private Animator animator;
    [HideInInspector] public UI_Cursor asignedCursor;
    private int enteredCursors;

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    public UI_SelectionCard GetSelected(int playerID, UI_Cursor cursor, UI_SelectionCard selectedCard)
    {
        if (asignedCursor == null)
        {
            // Quitar la selección anterior
            if (selectedCard != null && selectedCard.asignedCursor == cursor)
            {
                selectedCard.animator.SetInteger("option", 0);
                selectedCard.asignedCursor = null;
            }

            // Seleccionar personaje
            animator.SetInteger("option", playerID + 2);
            asignedCursor = cursor;
            Debug.Log("Player " + playerID + " selected " + gameObject.name);
            asignedCursor.playerController.ChangeCharacter(characterID);

            return this;
        }
        else
        { return selectedCard;}
    }

    public void Deselect()
    {
        animator.SetInteger("option", 0);
        asignedCursor = null;
    }

    private void OnTriggerEnter2D(Collider2D tri)
    {
        if (tri.gameObject.name == "Cursor")
        {
            enteredCursors++;

            UI_Cursor cursor = tri.gameObject.GetComponent<UI_Cursor>();
            Mov_Player_Controller playerController = cursor.playerController;

            cursor.hoveredCard = this;

            if (asignedCursor == null)
            {
                animator.SetInteger("option", 1);
            }
        }
    }


    private void OnTriggerExit2D(Collider2D tri)
    {
        if (tri.gameObject.name == "Cursor")
        {
            enteredCursors--;

            UI_Cursor cursor = tri.gameObject.GetComponent<UI_Cursor>();
            Mov_Player_Controller playerController = cursor.playerController;

            cursor.hoveredCard = null;

            if (asignedCursor == null && enteredCursors == 0)
            {
                animator.SetInteger("option", 0);
            }
        }
    }
}
