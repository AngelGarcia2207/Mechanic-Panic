using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Esta es la clase para los objetos de armas tirados en el suelo.
*/

public class Obj_Weapon_Item : Obj_Item
{
    //Esta propiedad puede ser un scriptable object de base o de complemento
    [SerializeField] Obj_Weapon_Component itemData;
    [SerializeField] public GameObject weaponPrefab;
    [SerializeField] public bool isPlayer = false;

    void Awake()
    {
        if(itemData is Obj_Weapon_Base)
        {
            Obj_Weapon_Base temp = itemData as Obj_Weapon_Base;
            temp.InitializeComplementLocations();
        }
        
    }

    //Función para aregar un outline al objeto cuando se aproxima un jugador
    protected override void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && other is not CharacterController)
        {
            Obj_Player_Weapon playerWeaponScript = other.gameObject.GetComponent<Mov_Player_Controller>().playerWeaponScript;
            
            if(playerWeaponScript.HasBase() == false && itemData is Obj_Weapon_Base)
            {
                playerWeaponScript.AddCloseItem(this);
            }
            
            if(playerWeaponScript.HasBase() && itemData is Obj_Weapon_Complement && playerWeaponScript.CheckIfFits(this))
            {
                playerWeaponScript.AddCloseItem(this);
            }
        }
    }

    //Función para quitar el outline al objeto cuando se aleja un jugador
    protected override void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player") && other is not CharacterController)
        {
            Obj_Player_Weapon playerWeaponScript = other.gameObject.GetComponent<Mov_Player_Controller>().playerWeaponScript;
            playerWeaponScript.RemoveCloseItem(this);
        }
    }

    public Obj_Weapon_Component GetData() { return itemData; }
}