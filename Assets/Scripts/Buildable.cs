using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildable : MonoBehaviour
{
    [SerializeField] protected int Weight;
    [SerializeField] protected int Size;
    [SerializeField] protected int Integrity;
    [SerializeField] protected MeshFilter buildableMesh;
    [SerializeField] protected MeshRenderer buildableRenderer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
