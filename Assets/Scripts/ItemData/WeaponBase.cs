using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponTypes
{
    Default,
    Swing,
    Poke,
    Shoot
}

[CreateAssetMenu(fileName = "WeaponBase", menuName = "WeaponBase", order = 0)]
public class WeaponBase : WeaponComponent
{
    [SerializeField] protected WeaponTypes attackType = WeaponTypes.Default;
}
