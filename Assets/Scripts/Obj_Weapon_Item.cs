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
            Obj_Player_Weapon playerWeapon = other.gameObject.transform.GetChild(other.gameObject.transform.childCount-1).GetChild(0).GetComponent<Obj_Player_Weapon>();
            
            if(playerWeapon.HasBase() == false && itemData is Obj_Weapon_Base)
            {
                playerWeapon.AddCloseItem(this);
            }
            
            if(playerWeapon.HasBase() && itemData is Obj_Weapon_Complement && playerWeapon.CheckIfFits(this))
            {
                playerWeapon.AddCloseItem(this);
            }
        }
    }

    //Función para quitar el outline al objeto cuando se aleja un jugador
    protected override void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player") && other is not CharacterController)
        {
            Obj_Player_Weapon playerWeapon = other.gameObject.transform.GetChild(other.gameObject.transform.childCount-1).GetChild(0).GetComponent<Obj_Player_Weapon>();
            playerWeapon.RemoveCloseItem(this);
            Debug.Log("Here");
        }
    }

    public void DestroyThis() { Destroy(this.gameObject); }
    public Obj_Weapon_Component GetData() { return itemData; }
    public MeshFilter GetMesh() { return itemMesh; }
    public MeshRenderer GetRenderer() { return itemRenderer; }
    public Collider GetCollider() { return gameObject.GetComponent<Collider>(); }
}