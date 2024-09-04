using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] protected Collider pickUpRadius;
    [SerializeField] protected MeshFilter itemMesh;
    [SerializeField] protected MeshRenderer itemRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && other is not CharacterController)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.pickUp.AddListener(OnPickUp);
            this.gameObject.layer = LayerMask.NameToLayer("Outline-Items");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.pickUp.RemoveListener(OnPickUp);
            this.gameObject.layer = LayerMask.NameToLayer("Non-Outline-Items");
        }
    }

    public virtual void OnPickUp(Weapon playerWeapon)
    {
        //
    }
}
