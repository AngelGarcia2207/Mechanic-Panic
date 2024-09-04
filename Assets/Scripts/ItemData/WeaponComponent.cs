using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponComponent : ScriptableObject
{
    [SerializeField] protected int Damage;
    [SerializeField] protected int Weight;
    [SerializeField] protected int Size;
    [SerializeField] protected int Complexity;
}