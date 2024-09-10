using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Esta clase es la plantilla para crear items de arma complementarios.
*/

[CreateAssetMenu(fileName = "WeaponComplement", menuName = "WeaponComplement", order = 0)]
public class Obj_Weapon_Complement : Obj_Weapon_Component
{
    [SerializeField] protected List<WeaponEffect> effects;
}
