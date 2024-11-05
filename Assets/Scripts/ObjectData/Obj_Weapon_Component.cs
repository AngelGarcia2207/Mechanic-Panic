using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Esta clase es la base para las plantillas de objetos de arma.
*/

public abstract class Obj_Weapon_Component : ScriptableObject
{
    [SerializeField] protected int Damage;
    [SerializeField] protected int Weight;
    [SerializeField] protected int Size;
    [SerializeField] protected int Complexity;
    [SerializeField] protected int Durability;

    public int GetDamage() { return Damage; }
    public int GetDurability() { return Durability; }
    public void DecreaseDurability(int amount) { Durability -= amount; }
}