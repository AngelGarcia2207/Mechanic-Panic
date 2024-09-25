using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Esta clase es la plantilla para crear items de arma complementarios.
*/

public enum ComplementPositions
{
    Default,
    Top,
    Front,
    Right,
    Left,
    Back,
    Center
}

[CreateAssetMenu(fileName = "WeaponComplement", menuName = "WeaponComplement", order = 0)]
public class Obj_Weapon_Complement : Obj_Weapon_Component
{
    [SerializeField] protected List<WeaponEffect> effects;
    [SerializeField] protected List<ComplementPositions> possiblePositions;

    public ComplementPositions GetBestPosition(Dictionary<ComplementPositions, int> elementsInPosition)
    {
        ComplementPositions bestPosition = ComplementPositions.Default;
        int counter = 999;
       
        foreach(ComplementPositions possiblePosition in possiblePositions)
        {
            if(elementsInPosition.ContainsKey(possiblePosition))
            {
                if(elementsInPosition[possiblePosition] < counter)
                {
                    bestPosition = possiblePosition;
                    counter = elementsInPosition[possiblePosition];
                }
            }
        }

        elementsInPosition[bestPosition]++;
        return bestPosition;
    }
}
