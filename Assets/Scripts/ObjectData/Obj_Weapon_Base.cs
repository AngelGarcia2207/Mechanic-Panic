using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Esta clase es la plantilla para crear items de arma base.
*/

public enum WeaponTypes
{
    Default,
    Swing,
    Poke,
    Shoot
}

[CreateAssetMenu(fileName = "WeaponBase", menuName = "WeaponBase", order = 0)]
public class Obj_Weapon_Base : Obj_Weapon_Component
{
    [SerializeField] protected WeaponTypes attackType = WeaponTypes.Default;
    [SerializeField] protected Vector3 complementPosition;
    [SerializeField] protected Vector3 positionChange;
    
    //Getters
    public Vector3 GetComplementPosition() {return complementPosition;}
    public Vector3 GetPositionChange() {return positionChange;}
}
