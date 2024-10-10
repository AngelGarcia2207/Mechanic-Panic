using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Find_Nearest : MonoBehaviour
{
    public GameObject FindNearest(GameObject startPositionObj, GameObject[] objectsList)
    {
        float shortestDistance = Mathf.Infinity;
        GameObject chosenObject = null;

        foreach (GameObject indObject in objectsList)
        {
            float actualDistance = Vector3.Distance(startPositionObj.transform.position, indObject.transform.position);
            if (actualDistance < shortestDistance)
            {
                shortestDistance = actualDistance;
                chosenObject = indObject;
            }
        }

        return chosenObject;
    }
}