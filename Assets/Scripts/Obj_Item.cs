using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Esta es la clase base para todos los objetos tirados en el suelo.
Todos esos items deben poseer estas propiedades.
*/

public abstract class Obj_Item : MonoBehaviour
{
    [SerializeField] protected Collider pickUpRadius;
    [SerializeField] protected MeshFilter itemMesh;
    [SerializeField] protected MeshRenderer itemRenderer;

    void Start()
    {
    }

    void Update()
    {
    }

    //Función para aregar un outline al objeto cuando se aproxima un jugador
    protected virtual void OnTriggerEnter(Collider other) {}

    //Función para quitar el outline al objeto cuando se aleja un jugador
    protected virtual void OnTriggerExit(Collider other) {}
}