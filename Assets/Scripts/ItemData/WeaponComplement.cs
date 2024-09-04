using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponComplement", menuName = "WeaponComplement", order = 0)]
public class WeaponComplement : WeaponComponent
{
    [SerializeField] protected List<WeaponEffect> effects;
}
