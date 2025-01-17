using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_Play_Selection : MonoBehaviour
{
    [SerializeField] private UI_SelectionCard[] selectionCards;
    [SerializeField] private GameObject characterSelectionMenu, chooseTextObj;
    private int enteredCursors;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Play()
    {
        int playersAsigned = 0;
        foreach (UI_SelectionCard card in selectionCards)
        {
            if (card.asignedCursor != null)
            {
                playersAsigned++;
            }
        }

        Mov_Player_Controller[] playerControllers = GameObject.FindObjectsOfType<Mov_Player_Controller>();

        if(playersAsigned == playerControllers.Length)
        {
            foreach (Mov_Player_Controller playerController in playerControllers)
            {
                playerController.finishedSelection = true;
            }

            try { GameObject.FindFirstObjectByType<PlayerInputManager>().enabled = false; }
            catch { }
            characterSelectionMenu.SetActive(false);
        }
        else
        {
            chooseTextObj.SetActive(false);
            chooseTextObj.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D tri)
    {
        if (tri.gameObject.name == "Cursor")
        {
            enteredCursors++;

            UI_Cursor cursor = tri.gameObject.GetComponent<UI_Cursor>();
            Mov_Player_Controller playerController = cursor.playerController;

            cursor.hoveredPlayButton = this;

            animator.SetBool("hovering", true);
        }
    }


    private void OnTriggerExit2D(Collider2D tri)
    {
        if (tri.gameObject.name == "Cursor")
        {
            enteredCursors--;

            UI_Cursor cursor = tri.gameObject.GetComponent<UI_Cursor>();
            Mov_Player_Controller playerController = cursor.playerController;

            cursor.hoveredPlayButton = null;

            if (enteredCursors == 0)
            {
                animator.SetBool("hovering", false);
            }
        }
    }
}
