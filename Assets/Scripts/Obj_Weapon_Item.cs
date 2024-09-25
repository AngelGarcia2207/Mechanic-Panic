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

    //Función para aregar un outline al objeto cuando se aproxima un jugador
    protected override void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && other is not CharacterController)
        {
            Mov_Player_Controller player = other.GetComponent<Mov_Player_Controller>();
            player.pickUp.AddListener(OnPickUp);
            this.gameObject.layer = LayerMask.NameToLayer("Outline-Items");
        }
    }

    //Función para quitar el outline al objeto cuando se aleja un jugador
    protected override void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player") && other is not CharacterController)
        {
            Mov_Player_Controller player = other.GetComponent<Mov_Player_Controller>();
            player.pickUp.RemoveListener(OnPickUp);
            this.gameObject.layer = LayerMask.NameToLayer("Non-Outline-Items");
        }
    }

    //Adaptación del método PickUp para armas
    public override void OnPickUp(Obj_Weapon playerWeapon)
    {
        if(itemData is Obj_Weapon_Base)
        {
            playerWeapon.SetBase(itemData as Obj_Weapon_Base, itemMesh, itemRenderer);
        }
        else
        {
            playerWeapon.AddComplement(itemData as Obj_Weapon_Complement, itemMesh, itemRenderer);
        }
        Destroy(this.gameObject);
    }
}