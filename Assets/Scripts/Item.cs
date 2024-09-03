using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] protected BoxCollider pickUpRadius;
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.pickUp.RemoveListener(OnPickUp);
        }
    }

    public void OnPickUp(Weapon playerWeapon)
    {
        playerWeapon.UpdateMesh(itemMesh, itemRenderer);
        Destroy(this.gameObject);
    }
}
