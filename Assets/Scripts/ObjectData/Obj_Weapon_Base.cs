using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Esta clase es la plantilla para crear items de arma base.
*/

public enum WeaponTypes
{
    Default,
    Swing,
    Poke,
    Shoot
}

[CreateAssetMenu(fileName = "WeaponBase", menuName = "WeaponBase", order = 0)]
public class Obj_Weapon_Base : Obj_Weapon_Component
{
    [SerializeField] protected WeaponTypes attackType = WeaponTypes.Default;
    [SerializeField] protected List<ComplementLocationHandler> complementLocations;
    
    //Buscar el index de cierto ComplementLocationHandler a partir de un tipo de locación dada.
    //NOTA: no, "IndexOf" no funciona para esto porque tendría que crear un ComplementLocationHandler con los mismos datos
    public int GetLocationIndex(ComplementLocations targetLocation)
    {
        for(int i = 0; i < complementLocations.Count; i++)
        {
            if(complementLocations[i].location == targetLocation)
            {
                return i;
            }
        }

        return -1;
    }

    public List<Vector3> GetInitialPositions(Dictionary<ComplementLocations, int> numberOfElementsPerLocation)
    {
        List<Vector3> initialPositions = new();

        foreach(ComplementLocationHandler locationHandler in complementLocations)
        {
            if(locationHandler.location == ComplementLocations.Front || locationHandler.location == ComplementLocations.Back)
            {
                initialPositions.Add(new Vector3(locationHandler.thirdValue, locationHandler.minLong, locationHandler.minTall));
            }
            else if(locationHandler.location == ComplementLocations.Right || locationHandler.location == ComplementLocations.Left)
            {
                initialPositions.Add(new Vector3(locationHandler.minTall, locationHandler.minLong, locationHandler.thirdValue));
            }
            else
            {
                initialPositions.Add(new Vector3(0, 0, 0));
            }
            numberOfElementsPerLocation.Add(locationHandler.location, 0);
        }

        return initialPositions;
    }

    public Vector3 GetNewComplementPosition(ComplementLocations newLocation, Vector3 currentPosition, Vector3 meshSize)
    {
        int newLocationIndex = GetLocationIndex(newLocation);

        switch(newLocation)
        {
            case ComplementLocations.Front:
                Debug.Log(meshSize.z/2);
                if(currentPosition.y > complementLocations[newLocationIndex].maxLong)
                {
                    if(currentPosition.z < complementLocations[newLocationIndex].maxTall)
                    {
                        return new Vector3(0, 0, 0);
                    }
                    else
                    {
                        return new Vector3(currentPosition.x, (complementLocations[newLocationIndex].minLong + meshSize.y/2),
                            (currentPosition.z - meshSize.z/2));
                    }
                }
                else
                {
                    return new Vector3(currentPosition.x, (currentPosition.y + meshSize.y/2),
                        (complementLocations[newLocationIndex].minTall - meshSize.z/2));
                }
                
            case ComplementLocations.Right:
                if(currentPosition.y > complementLocations[newLocationIndex].maxLong)
                {
                    if(currentPosition.x > complementLocations[newLocationIndex].maxTall)
                    {
                        return new Vector3(0, 0, 0);
                    }
                    else
                    {
                        return new Vector3((currentPosition.x + meshSize.z/2),
                            (complementLocations[newLocationIndex].minLong + meshSize.y/2), currentPosition.z);
                    }
                }
                else
                {
                    return new Vector3((complementLocations[newLocationIndex].minTall + meshSize.z/2),
                        (currentPosition.y + meshSize.y/2), currentPosition.z);
                }
                
            case ComplementLocations.Left:
                if(currentPosition.y > complementLocations[newLocationIndex].maxLong)
                {
                    if(currentPosition.x < complementLocations[newLocationIndex].maxTall)
                    {
                        return new Vector3(0, 0, 0);
                    }
                    else
                    {
                        return new Vector3((currentPosition.x - meshSize.z/2),
                            (complementLocations[newLocationIndex].minLong + meshSize.y/2), currentPosition.z);
                    }
                }
                else
                {
                    return new Vector3((complementLocations[newLocationIndex].minTall - meshSize.z/2),
                        (currentPosition.y + meshSize.y/2), currentPosition.z);
                }
                
            case ComplementLocations.Back:
                if(currentPosition.y > complementLocations[newLocationIndex].maxLong)
                {
                    if(currentPosition.z > complementLocations[newLocationIndex].maxTall)
                    {
                        return new Vector3(0, 0, 0);
                    }
                    else
                    {
                        return new Vector3(currentPosition.x, (complementLocations[newLocationIndex].minLong + meshSize.y/2),
                            (currentPosition.z + meshSize.z/2));
                    }
                }
                else
                {
                    return new Vector3(currentPosition.x, (currentPosition.y + meshSize.y/2),
                        (complementLocations[newLocationIndex].minTall + meshSize.z/2));
                }

            default:
                return currentPosition;
        }
    }

    public Vector3 GetAngleChange(int locationIndex)
    {
        return complementLocations[locationIndex].angleChange;
    }

    public void InitializeComplementLocations()
    {
        //
    }
}

[System.Serializable]
public struct ComplementLocationHandler
{
    public ComplementLocations location;
    public float minLong;
    public float maxLong;
    public float minTall;
    public float maxTall;
    public float thirdValue;
    public Vector3 angleChange;
}