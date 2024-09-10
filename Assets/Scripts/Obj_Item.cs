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

    //Función para aregar un outline al objeto cuando se aproxima un jugador
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && other is not CharacterController)
        {
            Mov_Player_Controller player = other.GetComponent<Mov_Player_Controller>();
            player.pickUp.AddListener(OnPickUp);
            this.gameObject.layer = LayerMask.NameToLayer("Outline-Items");
        }
    }

    //Función para quitar el outline al objeto cuando se aleja un jugador
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Mov_Player_Controller player = other.GetComponent<Mov_Player_Controller>();
            player.pickUp.RemoveListener(OnPickUp);
            this.gameObject.layer = LayerMask.NameToLayer("Non-Outline-Items");
        }
    }

    //Función base de PickUp. Cada tipo de objeto implementa su propio método.
    public virtual void OnPickUp(Obj_Weapon playerWeapon) {}
}