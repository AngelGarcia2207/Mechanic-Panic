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
    [SerializeField] protected List<PositionHandler> complementPositions;
    
    //Buscar el index de cierto PositionHandler a partir de un tipo de posición dada.
    //NOTA: no, "IndexOf" no funciona para esto porque tendría que crear un PositionHandler con los mismos datos
    public int GetPositionIndex(ComplementPositions targetPosition)
    {
        for(int i = 0; i < complementPositions.Count; i++)
        {
            if(complementPositions[i].position == targetPosition)
            {
                return i;
            }
        }

        return -1;
    }

    public List<Vector3> GetInitialPositions(Dictionary<ComplementPositions, int> elementsInPosition)
    {
        List<Vector3> initialPositions = new();

        foreach(PositionHandler position in complementPositions)
        {
            initialPositions.Add(position.initialPosition);
            elementsInPosition.Add(position.position, 0);
        }

        return initialPositions;
    }

    public Vector3 GetNewComplementPosition(ComplementPositions newPosition, Vector3 currentPosition, Vector3 meshSize)
    {
        int newPositionIndex = GetPositionIndex(newPosition);

        switch(newPosition)
        {
            case ComplementPositions.Front:
                if(currentPosition.y + meshSize.y > complementPositions[newPositionIndex].maxValues.y)
                {
                    if(complementPositions[newPositionIndex].initialPosition.z - meshSize.z < complementPositions[newPositionIndex].maxValues.z)
                    {
                        return new Vector3(0, 0, 0);
                    }
                    else
                    {
                        return new Vector3(currentPosition.x, (complementPositions[newPositionIndex].initialPosition.y + meshSize.y/2),
                            (complementPositions[newPositionIndex].initialPosition.z - 1.5f*meshSize.z));
                    }
                }
                else
                {
                    return new Vector3(currentPosition.x, (currentPosition.y + meshSize.y/2), (currentPosition.z - meshSize.z/2));
                }
                
            case ComplementPositions.Right:
                if(currentPosition.y + meshSize.y > complementPositions[newPositionIndex].maxValues.y)
                {
                    if(complementPositions[newPositionIndex].initialPosition.x + meshSize.z > complementPositions[newPositionIndex].maxValues.z)
                    {
                        return new Vector3(0, 0, 0);
                    }
                    else
                    {
                        return new Vector3((complementPositions[newPositionIndex].initialPosition.x + 1.5f*meshSize.z),
                            (complementPositions[newPositionIndex].initialPosition.y + meshSize.y/2), currentPosition.z);
                    }
                }
                else
                {
                    return new Vector3((currentPosition.x + meshSize.z/2), (currentPosition.y + meshSize.y/2), currentPosition.z);
                }
                
            case ComplementPositions.Left:
                if(currentPosition.y + meshSize.y > complementPositions[newPositionIndex].maxValues.y)
                {
                    if(complementPositions[newPositionIndex].initialPosition.x - meshSize.z < complementPositions[newPositionIndex].maxValues.z)
                    {
                        return new Vector3(0, 0, 0);
                    }
                    else
                    {
                        return new Vector3((complementPositions[newPositionIndex].initialPosition.x - 1.5f*meshSize.z),
                            (complementPositions[newPositionIndex].initialPosition.y + meshSize.y/2), currentPosition.z);
                    }
                }
                else
                {
                    return new Vector3((currentPosition.x - meshSize.z/2), (currentPosition.y + meshSize.y/2), currentPosition.z);
                }
                
            case ComplementPositions.Back:
                if(currentPosition.y + meshSize.y > complementPositions[newPositionIndex].maxValues.y)
                {
                    if(complementPositions[newPositionIndex].initialPosition.z + meshSize.z > complementPositions[newPositionIndex].maxValues.z)
                    {
                        return new Vector3(0, 0, 0);
                    }
                    else
                    {
                        return new Vector3(currentPosition.x, (complementPositions[newPositionIndex].initialPosition.y + meshSize.y/2),
                            (complementPositions[newPositionIndex].initialPosition.z + 1.5f*meshSize.z));
                    }
                }
                else
                {
                    return new Vector3(currentPosition.x, (currentPosition.y + meshSize.y/2), (currentPosition.z + meshSize.z/2));
                }

            default:
                return currentPosition;
        }
    }

    public Vector3 GetAngleChange(int positionIndex)
    {
        return complementPositions[positionIndex].angleChange;
    }
}

[System.Serializable]
public struct PositionHandler
{
    [SerializeField] public ComplementPositions position;
    [SerializeField] public Vector3 initialPosition;
    [SerializeField] public Vector3 maxValues;
    [SerializeField] public Vector3 angleChange;
}