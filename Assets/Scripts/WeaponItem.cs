using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : Item
{
    [SerializeField] WeaponComponent itemData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPickUp(Weapon playerWeapon)
    {
        if(itemData is WeaponBase)
        {
            playerWeapon.SetBase(itemData as WeaponBase, itemMesh, itemRenderer);
        }
        else
        {
            playerWeapon.AddComplement(itemData as WeaponComplement, itemMesh, itemRenderer);
        }
        Destroy(this.gameObject);
    }
}