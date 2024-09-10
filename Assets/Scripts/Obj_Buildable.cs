using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Esta es la clase base para todos los objetos que el jugador usa/lleva encima.
Todos esos objetos deben poseer estas propiedades.
*/

public abstract class Obj_Buildable : MonoBehaviour
{
    //Stats
    [SerializeField] protected int Weight;
    [SerializeField] protected int Size;
    [SerializeField] protected int Integrity;

    //Rendering
    [SerializeField] protected MeshFilter buildableMesh;
    [SerializeField] protected MeshRenderer buildableRenderer;
    [SerializeField] protected Vector3 complementPosition;
    [SerializeField] protected Vector3 positionChange;
}
