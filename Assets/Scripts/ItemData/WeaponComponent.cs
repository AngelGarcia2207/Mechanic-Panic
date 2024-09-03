using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponComponent", menuName = "WeaponComponent", order = 0)]
public class WeaponComponent : ScriptableObject
{
    protected int Damage;
    protected int Weight;
    protected int Size;
    protected int Complexity;
}
