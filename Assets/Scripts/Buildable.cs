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
    [SerializeField] protected Vector3 complementPosition;
    [SerializeField] protected Vector3 positionChange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
