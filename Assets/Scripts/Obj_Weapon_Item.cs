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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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