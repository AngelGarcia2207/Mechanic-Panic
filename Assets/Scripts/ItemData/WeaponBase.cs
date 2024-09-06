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
    [SerializeField] protected Vector3 complementPosition;
    [SerializeField] protected Vector3 positionChange;

    public Vector3 GetComplementPosition()
    {
        return complementPosition;
    }

    public Vector3 GetPositionChange()
    {
        return positionChange;
    }
}
