using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_SimpleChunk : Map_ChunkManager
{
    [SerializeField] protected GameObject stickPrefab;
    [SerializeField] protected Map_ItemSpawner itemSpawner;

    public override void PopulateChunk(float randVal)
    {
        if(randVal < 0.3f)
        {
            Destroy(fence);
        }

        int chanceTrees = (int)(randVal*100) % 5;
        int chanceTreeDrop = ((int)(randVal*10) % 10) + ((int)(randVal*1000000) % 10);
        GameObject newTree;

        switch(chanceTrees)
        {
            case 0:
                //Tree on the left
                newTree = Instantiate(treePrefab, treeReference.position, Quaternion.identity, transform);
                newTree.transform.localPosition += new Vector3(((randVal*1000)%10)/5, 0, -((randVal*10000)%10)/5);
                if(chanceTreeDrop > 4 && chanceTreeDrop < 14)
                {
                    GameObject newStick = Instantiate(stickPrefab, newTree.transform.GetChild(0).GetChild(0).position, Quaternion.identity, transform);
                    newStick.transform.localEulerAngles = new Vector3(-90, 0, 0);
                }
                break;
            
            case 1:
                //Tree on the middle
                newTree = Instantiate(treePrefab, treeReference.position, Quaternion.identity, transform);
                newTree.transform.localPosition += new Vector3(3+((randVal*1000)%10)/5, 0, -((randVal*10000)%10)/5);
                if(chanceTreeDrop > 4 && chanceTreeDrop < 14)
                {
                    GameObject newStick = Instantiate(stickPrefab, newTree.transform.GetChild(0).GetChild(1).position, Quaternion.identity, transform);
                    newStick.transform.localEulerAngles = new Vector3(-90, 90, 0);
                }
                break;
            
            case 2:
                //Tree on the right
                newTree = Instantiate(treePrefab, treeReference.position, Quaternion.identity, transform);
                newTree.transform.localPosition += new Vector3(6+((randVal*1000)%10)/5, 0, -((randVal*10000)%10)/5);
                if(chanceTreeDrop > 4 && chanceTreeDrop < 14)
                {
                    GameObject newStick = Instantiate(stickPrefab, newTree.transform.GetChild(0).GetChild(2).position, Quaternion.identity, transform);
                    newStick.transform.localEulerAngles = new Vector3(-90, 0, 0);
                }
                break;
            
            case 3:
                //Tree on the left and right
                newTree = Instantiate(treePrefab, treeReference.position, Quaternion.identity, transform);
                newTree.transform.localPosition += new Vector3(((randVal*1000)%10)/5, 0, -((randVal*10000)%10)/5);
                if(chanceTreeDrop > 4 && chanceTreeDrop < 9)
                {
                    GameObject newStick = Instantiate(stickPrefab, newTree.transform.GetChild(0).GetChild(0).position, Quaternion.identity, transform);
                    newStick.transform.localEulerAngles = new Vector3(-90, 0, 0);
                }
                newTree = Instantiate(treePrefab, treeReference.position, Quaternion.identity, transform);
                newTree.transform.localPosition += new Vector3(6+((randVal*1000)%10)/5, 0, -((randVal*10000)%10)/5);
                if(chanceTreeDrop > 9 && chanceTreeDrop < 14)
                {
                    GameObject newStick = Instantiate(stickPrefab, newTree.transform.GetChild(0).GetChild(2).position, Quaternion.identity, transform);
                    newStick.transform.localEulerAngles = new Vector3(-90, 0, 0);
                }
                break;
            
            case 4:
                //No trees
                break;
        }

        Transform reference = grassNRocksReference;
        int objCounter = 0;

        for(int i = 1; i < 7; i++)
        {
            int twoDigits = (int)(randVal*Mathf.Pow(10, i));
            int d1 = twoDigits % 10;
            int d2 = (twoDigits/10) % 10;
            int chanceGrassRocks = d1 + d2;

            if(chanceGrassRocks > 7 && chanceGrassRocks < 11)
            {
                //Nothing
                objCounter += 2;
            }
            else if(chanceGrassRocks > 3 && chanceGrassRocks <= 7)
            {
                //Only Grass
                GameObject newGrass = Instantiate(grassPrefab, reference.position, Quaternion.identity, transform);

                if(d1 > d2)
                {
                    //Place on the left
                    newGrass.transform.localPosition += new Vector3(((d1*i)%10)/5, 0, -((d2*i)%10)/20);
                }
                else if(d2 > d1)
                {
                    //Place on the right
                    newGrass.transform.localPosition += new Vector3(4+((d1*i)%10)/5, 0, -((d2*i)%10)/20);
                }
                else
                {
                    //Place in the middle
                    newGrass.transform.localPosition += new Vector3(2+((d1*i)%10)/5, 0, -((d2*i)%10)/20);
                }

                objCounter++;
            }
            else if(chanceGrassRocks >= 11 && chanceGrassRocks < 15)
            {
                //Only rock
                GameObject newRock = Instantiate(rockPrefab, reference.position, Quaternion.identity, transform);
                newRock.transform.localScale = new Vector3(56, 56, 56);

                if(d1 > d2)
                {
                    //Place on the left
                    newRock.transform.localPosition += new Vector3(((d2*i)%10)/5, 0, -((d1*i)%10)/20);
                }
                else if(d2 > d1)
                {
                    //Place on the right
                    newRock.transform.localPosition += new Vector3(4+((d2*i)%10)/5, 0, -((d1*i)%10)/20);
                }
                else
                {
                    //Place in the middle
                    newRock.transform.localPosition += new Vector3(2+((d2*i)%10)/5, 0, -((d1*i)%10)/20);
                }

                objCounter++;
            }
            else
            {
                //Both grass and rock
                GameObject newGrass = Instantiate(grassPrefab, reference.position, Quaternion.identity, transform);
                GameObject newRock = Instantiate(rockPrefab, reference.position, Quaternion.identity, transform);
                newRock.transform.localScale = new Vector3(42, 42, 42);

                if(d1 == d2)
                {
                    //Make sure they are not equal
                    if(i%2 == 0)
                    {
                        d1++;
                    }
                    else
                    {
                        d2++;
                    }
                }

                if(d1 > d2)
                {
                    //Place grass on the left, rock on the right
                    newGrass.transform.localPosition += new Vector3(((d1*i)%10)/4, 0, -((d2*i)%10)/20);
                    newRock.transform.localPosition += new Vector3(3+((d2*i)%10)/4, 0, -((d1*i)%10)/20);
                }
                else if(d2 > d1)
                {
                    //Place rock on the left, grass on the right
                    newRock.transform.localPosition += new Vector3(((d2*i)%10)/4, 0, -((d1*i)%10)/20);
                    newGrass.transform.localPosition += new Vector3(3+((d1*i)%10)/4, 0, -((d2*i)%10)/20);
                }
            }

            reference.localPosition -= new Vector3(0, 0, 1);
        }

        if(objCounter > 9)
        {
            itemSpawner.SpawnItems(3, randVal);
        }
        else if(objCounter > 6)
        {
            itemSpawner.SpawnItems(2, randVal);
        }
        else if(objCounter > 2)
        {
            itemSpawner.SpawnItems(1, randVal);
        }
    }
}
