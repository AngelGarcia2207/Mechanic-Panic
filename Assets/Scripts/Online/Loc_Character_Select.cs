using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loc_Character_Select : MonoBehaviour
{
    [HideInInspector] public Onl_Player_Controller onlController;

    private Onl_Character_Select onlCharSelect;

    private int selectedCharacter = 0;

    void Start()
    {
        if(GetComponent<Onl_Character_Select>() != null)
        {
            onlCharSelect = GetComponent<Onl_Character_Select>();
        }
    }

    public void SelectCharacter(int character)
    {
        if (onlController != null)
        {
            Debug.Log("onlController SI existe");
            onlController.TryOnlineChooseCharacter(character);
            if (onlCharSelect != null)
            {
                onlCharSelect.CharacterCrossSelect(character, selectedCharacter);
                selectedCharacter = character;
            }
        }
    }
}
