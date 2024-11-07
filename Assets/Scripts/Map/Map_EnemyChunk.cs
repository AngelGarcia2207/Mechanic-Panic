using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_EnemyChunk : Map_ChunkManager
{
    [SerializeField] protected GameObject fence2;

    public override void PopulateChunk(float randVal)
    {
        if(((int)(randVal*10))%10 > 4)
        {
            Destroy(fence);
        }
        
        if(((int)(randVal*100000))%10 > 4)
        {
            Destroy(fence2);
        }

        int chanceTrees = (int)(randVal*100) % 5;
        GameObject newTree;

        switch(chanceTrees)
        {
            case 0:
                //Tree on the left
                newTree = Instantiate(treePrefab, treeReference.position, Quaternion.identity, transform);
                newTree.transform.localPosition += new Vector3(((randVal*1000)%10)/2, 0, -((randVal*10000)%10)/5);
                break;
            
            case 1:
                //Tree on the middle
                newTree = Instantiate(treePrefab, treeReference.position, Quaternion.identity, transform);
                newTree.transform.localPosition += new Vector3(6.5f+((randVal*1000)%10)/2, 0, -((randVal*10000)%10)/5);
                break;
            
            case 2:
                //Tree on the right
                newTree = Instantiate(treePrefab, treeReference.position, Quaternion.identity, transform);
                newTree.transform.localPosition += new Vector3(13.5f+((randVal*1000)%10)/2, 0, -((randVal*10000)%10)/5);
                break;
            
            case 3:
                //Tree on the left and right
                newTree = Instantiate(treePrefab, treeReference.position, Quaternion.identity, transform);
                newTree.transform.localPosition += new Vector3(((randVal*1000)%10)/2, 0, -((randVal*10000)%10)/5);
                newTree = Instantiate(treePrefab, treeReference.position, Quaternion.identity, transform);
                newTree.transform.localPosition += new Vector3(13.5f+((randVal*1000)%10)/2, 0, -((randVal*10000)%10)/5);
                break;
            
            case 4:
                //No trees
                break;
        }
    }
}
