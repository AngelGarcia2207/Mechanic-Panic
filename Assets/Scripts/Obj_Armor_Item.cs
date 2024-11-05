using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_Armor_Item : Obj_Item
{
    //Esta propiedad puede ser un scriptable object de base o de complemento
    [SerializeField] Obj_Armor_Component itemData;

    void Awake()
    {   
    }

    //Función para aregar un outline al objeto cuando se aproxima un jugador
    protected override void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && other is not CharacterController)
        {
            Obj_Player_Armor playerArmor = null;

            for(int i = 0; i < other.transform.childCount; i++)
            {
                if(other.transform.GetChild(i).gameObject.activeSelf)
                {
                    playerArmor = other.transform.GetChild(i).GetChild(2).GetComponent<Obj_Player_Armor>();
                    break;
                }
            }
            
            if(playerArmor.HasBodyArmor() == false && itemData is Obj_Armor_Body)
            {
                playerArmor.AddCloseItem(this);
            }
            
            if(itemData is Obj_Armor_Hat)
            {
                playerArmor.AddCloseItem(this);
            }
        }
    }

    //Función para quitar el outline al objeto cuando se aleja un jugador
    protected override void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player") && other is not CharacterController)
        {
            Obj_Player_Armor playerArmor = null;

            for(int i = 0; i < other.transform.childCount; i++)
            {
                if(other.transform.GetChild(i).gameObject.activeSelf)
                {
                    playerArmor = other.transform.GetChild(i).GetChild(2).GetComponent<Obj_Player_Armor>();
                    break;
                }
            }

            playerArmor.RemoveCloseItem(this);
        }
    }
    
    public Obj_Armor_Component GetData() { return itemData; }
}
