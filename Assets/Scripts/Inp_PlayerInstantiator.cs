using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inp_PlayerInstantiator : MonoBehaviour
{
    [SerializeField] private GameObject[] characterObj;

    [HideInInspector] public bool jumpButtonPressed;

    void Start()
    {
        int amountPlayers = FindObjectsOfType<Inp_PlayerInstantiator>().Length;
        characterObj[amountPlayers - 1].SetActive(true);
        Map_Display_Boundaries.Instance.AddPlayer(characterObj[amountPlayers - 1]);
    }

    private void OnJump()
    {
        jumpButtonPressed = !jumpButtonPressed;
    }
}
