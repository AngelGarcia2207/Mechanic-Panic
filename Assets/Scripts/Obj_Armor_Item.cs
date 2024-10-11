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
            Obj_Player_Armor playerArmor = other.gameObject.transform.GetChild(other.gameObject.transform.childCount-1).GetComponent<Obj_Player_Armor>();
            
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
            Obj_Player_Armor playerArmor = other.gameObject.transform.GetChild(other.gameObject.transform.childCount-1).GetComponent<Obj_Player_Armor>();
            playerArmor.RemoveCloseItem(this);
        }
    }
    
    public Obj_Armor_Component GetData() { return itemData; }
}