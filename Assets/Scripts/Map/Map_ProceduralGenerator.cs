using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_ProceduralGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> chunkPool = new();
    [SerializeField] private int seed;
    [SerializeField] private int preloadedChunks;
    [SerializeField] private int maxChunks;
    [SerializeField] private Transform loadPositionReference;
    [SerializeField] private Map_ChunkManager initialChunk;
    private float[] chunkValues;
    private int chunkCounter = 0;

    void Start()
    {
        if(seed != 0)
        {
           Random.InitState(seed);
        }

        chunkValues = new float[maxChunks];

        for(int i = 0; i < maxChunks; i++)
        {
            chunkValues[i] = Random.value;
        }

        initialChunk.loadNewChunk.AddListener(LoadChunk);

        for(int i = 0; i < preloadedChunks; i++)
        {
            LoadChunk(null);
        }
    }

    public void LoadChunk(Map_ChunkManager oldChunk)
    {
        //float val = Random.value;
        float val = chunkValues[chunkCounter];
        int index = (int)(val*10) % chunkPool.Count;
        //Debug.Log(index);
        GameObject newChunk = Instantiate(chunkPool[index], loadPositionReference.position, Quaternion.identity, transform.GetChild(1));
        newChunk.GetComponent<Map_ChunkManager>().loadNewChunk.AddListener(LoadChunk);
        newChunk.GetComponent<Map_ChunkManager>().PopulateChunk(val);
        loadPositionReference.localPosition += new Vector3(newChunk.GetComponent<Map_ChunkManager>().chunkSize, 0, 0);
        if(oldChunk != null)
        {
            oldChunk.loadNewChunk.RemoveListener(LoadChunk);
        }
        chunkCounter++;
    }
}
