using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Esta clase es la plantilla para crear items de arma complementarios.
*/

public enum ComplementLocations
{
    Center,
    Front,
    Right,
    Left,
    Back
}

public enum ColliderType
{
    Box,
    Sphere,
    Mesh
}

[CreateAssetMenu(fileName = "WeaponComplement", menuName = "Weapons & Armor/Weapon Complement", order = 0)]
public class Obj_Weapon_Complement : Obj_Weapon_Component
{
    [SerializeField] protected List<WeaponEffect> effects;
    [SerializeField] protected List<ComplementLocations> possibleLocations;
    [SerializeField] protected ColliderType colliderType;

    public ColliderType GetColliderType() { return colliderType; }
    public List<ComplementLocations> GetLocationsList() { return possibleLocations; }

    //Returns the complement location with the least amount of elements
    public static ComplementLocations GetBestLocation(Dictionary<ComplementLocations, int> numberOfElementsPerLocation, List<ComplementLocations> possibleLocations)
    {
        ComplementLocations bestLocation = ComplementLocations.Center;
        int counter = 999;
       
        foreach(ComplementLocations possibleLocation in possibleLocations)
        {
            if(numberOfElementsPerLocation.ContainsKey(possibleLocation))
            {
                if(numberOfElementsPerLocation[possibleLocation] < counter)
                {
                    bestLocation = possibleLocation;
                    counter = numberOfElementsPerLocation[possibleLocation];
                }
            }
        }
        
        return bestLocation;
    }

    public void PlayEffects(Transform target)
    {
        foreach(WeaponEffect effect in effects)
        {
            effect.PlayEffect(target);
        }
    }
}
