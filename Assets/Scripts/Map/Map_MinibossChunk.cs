using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_MinibossChunk : Map_ChunkManager
{
    [SerializeField] protected GameObject fence2;
    [SerializeField] protected GameObject fence3;
    
    public override void PopulateChunk(float randVal)
    {
        if(((int)(randVal*10))%10 > 4)
        {
            Destroy(fence);
        }
        
        if(((int)(randVal*100))%10 > 4)
        {
            Destroy(fence2);
        }

        if(((int)(randVal*1000))%10 > 4)
        {
            Destroy(fence3);
        }
    }
}
