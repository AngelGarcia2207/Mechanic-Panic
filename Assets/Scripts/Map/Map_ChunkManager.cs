using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Map_ChunkManager : MonoBehaviour
{
    [SerializeField] protected bool isReadyToUnload = false;
    [SerializeField] protected BoxCollider loadCollider;
    [SerializeField] protected GameObject fence;
    [SerializeField] protected GameObject grassPrefab, treePrefab, rockPrefab;
    [SerializeField] protected Transform treeReference, grassNRocksReference;
    public int chunkSize;
    public UnityEvent<Map_ChunkManager> loadNewChunk;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("CameraCenter"))
        {
            if(isReadyToUnload == false)
            {
                loadNewChunk.Invoke(this);
                Destroy(loadCollider);
                isReadyToUnload = true;
            }
            else
            {
                Destroy(gameObject, 3f);
            }
        }
    }

    public virtual void PopulateChunk(float randVal)
    {
        //
    }
}
