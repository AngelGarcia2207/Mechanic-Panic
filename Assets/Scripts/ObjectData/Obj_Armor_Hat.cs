using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HatArmor", menuName = "HatArmor", order = 0)]
public class Obj_Armor_Hat : Obj_Armor_Component
{
    [SerializeField] protected bool isHollow;

    public Vector3 GetRotationVariation()
    {
        return new Vector3(0, 0, 0);
    }

    public bool CheckIfHollow() { return isHollow; }
}
