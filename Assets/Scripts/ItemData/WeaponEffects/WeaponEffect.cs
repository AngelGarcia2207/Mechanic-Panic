using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponEffect", menuName = "WeaponEffect", order = 0)]
public class WeaponEffect : ScriptableObject
{
    public virtual void PlayEffect(){}
}