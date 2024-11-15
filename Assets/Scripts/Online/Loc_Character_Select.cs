using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        if (onlController != null && onlCharSelect != null)
        {
            // El personaje que se clickeó, si está disponible
            if (GameObject.Find("selected_id_" + character).GetComponent<Image>().enabled == false)
            {
                onlController.TryOnlineChooseCharacter(character);
                onlCharSelect.CharacterCrossSelect(character, selectedCharacter);
                selectedCharacter = character;
                
                // Guardamos el character en base al ID del juaador
                onlController.SaveCharacter(character);
            }
        }
    }
}
