using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_ItemSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> itemList;

    public void SpawnItems(int amount, float randVal)
    {
        int[] temp = {0, 1, 2, 3, 4};
        List<int> availableSpawns = new List<int>(temp);

        for(int i = 0; i < amount; i++)
        {
            int product = (((int)(randVal*Mathf.Pow(10, i+1)) % 10)+1 * ((int)(randVal*Mathf.Pow(10, i+2)) % 10)+1 * ((int)(randVal*Mathf.Pow(10, i+3)) % 10)+1);
            int spawnPick = product % availableSpawns.Count;
            int itemPick = (product * (int)((randVal*1000000) % 10)) % itemList.Count;

            GameObject newItem = Instantiate(itemList[itemPick], transform.GetChild(availableSpawns[spawnPick]).position, Quaternion.identity, transform);
            newItem.transform.localEulerAngles = new Vector3(-90, 0, 0);
            availableSpawns.RemoveAt(spawnPick);
        }
    }
}
