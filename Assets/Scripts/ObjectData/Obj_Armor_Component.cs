using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Esta clase es la plantilla para crear items de armadura.
*/

public abstract class Obj_Armor_Component : ScriptableObject
{
    [SerializeField] protected int Defense;
    [SerializeField] protected int Weight;
    [SerializeField] protected List<ArmorEffect> effects;

    public int GetDefense() { return Defense; }
}
