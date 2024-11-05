using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Esta es la clase para las armaduras que lleva el jugador
*/

public class Obj_Player_Armor : Obj_Buildable
{
    [SerializeField] protected int defense;
    [SerializeField] protected Obj_Armor_Body bodyArmor;
    [SerializeField] protected List<Obj_Armor_Hat> hats;
    [SerializeField] protected GameObject hatPrefab;
    [SerializeField] protected Transform hatParent;
    [SerializeField] protected Vector3 currentHatPosition;
    protected int numberOfHats = 0;
    protected List<Obj_Armor_Item> closeItems = new();
    protected Obj_Armor_Item closestItem;
    
    void Start()
    {
        Mov_Player_Controller player = GameObject.FindWithTag("Player").GetComponent<Mov_Player_Controller>();
        player.pickUpArmor.AddListener(OnPickUp);
    }
    
    void Update()
    {
        if(closeItems.Count == 0)
        {
            return;
        }
        else if(closeItems.Count == 1)
        {
            closestItem = closeItems[0];
        }
        else
        {
            closestItem = GetClosestItem();
        }

        if(closestItem != null && closestItem.gameObject.layer != LayerMask.NameToLayer("Outline-Items"))
        {
            closestItem.gameObject.layer = LayerMask.NameToLayer("Outline-Items");
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            foreach(Obj_Armor_Item item in closeItems)
            {
                Debug.Log(item.gameObject.name);
            }
        }
    }

    //Add a body armor to the player
    public void SetBodyArmor(Obj_Armor_Item newBodyArmor)
    {
        bodyArmor = newBodyArmor.GetData() as Obj_Armor_Body;
        buildableMesh.mesh = newBodyArmor.GetMesh().mesh;
        buildableRenderer.materials = newBodyArmor.GetRenderer().materials;
        bodyArmor.UpdateMeshTransform(buildableMesh.transform);
        defense = bodyArmor.GetDefense();
        Collider armorCollider = gameObject.AddComponent(newBodyArmor.GetCollider().GetType()) as Collider;
        armorCollider.isTrigger = true;
        if(armorCollider is BoxCollider boxCollider)
        {
            BoxCollider newBoxCollider = newBodyArmor.GetCollider() as BoxCollider;
            boxCollider.center = newBoxCollider.center;
            boxCollider.size = newBoxCollider.size;
        }
    }

    //Add a complement to this weapon
    public void AddHat(Obj_Armor_Item newHat)
    {
        //Initial data
        Obj_Armor_Hat newData = newHat.GetData() as Obj_Armor_Hat;
        hats.Add(newData);
        numberOfHats++;
        /*foreach(Obj_Armor_Item item in closeItems)
        {
            Debug.Log(item);
        }*/

        //Make instance of hat and position it
        GameObject instantiatedComplement = Instantiate(hatPrefab, transform.position, new Quaternion(0, 0, 0, 0), hatParent);
        instantiatedComplement.transform.localPosition = new Vector3(currentHatPosition.x + newHat.GetMesh().mesh.bounds.size.z/2,
            currentHatPosition.y , currentHatPosition.z);
        newData.UpdateMeshTransform(instantiatedComplement.transform);
        
        //Update position
        currentHatPosition.x += newHat.GetMesh().mesh.bounds.size.z;

        //Render complement
        instantiatedComplement.GetComponent<MeshFilter>().mesh =  newHat.GetMesh().mesh;
        instantiatedComplement.GetComponent<MeshRenderer>().materials = newHat.GetRenderer().materials;

        //Add collisions
        Collider armorCollider = instantiatedComplement.AddComponent(newHat.GetCollider().GetType()) as Collider;
        armorCollider.isTrigger = true;
        if(armorCollider is BoxCollider boxCollider)
        {
            BoxCollider newBoxCollider = newHat.GetCollider() as BoxCollider;
            boxCollider.center = newBoxCollider.center;
            boxCollider.size = newBoxCollider.size;
        }
        else if(armorCollider is SphereCollider sphereCollider)
        {
            SphereCollider newSphereCollider = newHat.GetCollider() as SphereCollider;
            sphereCollider.center = newSphereCollider.center;
            sphereCollider.radius = newSphereCollider.radius;
        }
        else if(armorCollider is MeshCollider meshCollider)
        {
            MeshCollider newMeshCollider = newHat.GetCollider() as MeshCollider;
            meshCollider.convex = newMeshCollider.convex;
            meshCollider.sharedMesh = newMeshCollider.sharedMesh;
        }

        //Reload CloseItems
        closeItems.Remove(closestItem);
    }

    public void OnPickUp(Obj_Player_Armor playerArmor)
    {
        if(closeItems.Count == 0)
        {
            return;
        }

        if(closestItem.GetData() is Obj_Armor_Body)
        {
            SetBodyArmor(closestItem);
            closeItems.Remove(closestItem);
        }
        else
        {
            AddHat(closestItem);
        }

        closestItem.DestroyThis();
        closestItem = null;
    }

    public void AddCloseItem(Obj_Armor_Item newItem)
    {
        closeItems.Add(newItem);
    }

    public void RemoveCloseItem(Obj_Armor_Item exitItem)
    {
        if(closeItems.Contains(exitItem) == false)
        {
            return;
        }

        if(closestItem == exitItem)
        {
            closestItem.gameObject.layer = LayerMask.NameToLayer("Non-Outline-Items");
            closestItem = null;
        }
        
        closeItems.Remove(exitItem);
    }

    public Obj_Armor_Item GetClosestItem()
    {   
        Obj_Armor_Item winner = closeItems[0];
        Vector3 playerPosition = transform.parent.gameObject.transform.parent.gameObject.transform.position;

        /*foreach(Obj_Armor_Item closeItem in closeItems)
        {
            //Debug.Log(closeItem.gameObject.name + Vector3.Distance(playerPosition, closeItem.transform.position));
        }*/

        for(int i = 1; i < closeItems.Count; i++)
        {
            if(Vector3.Distance(playerPosition, closeItems[i].transform.position) <
                Vector3.Distance(playerPosition, winner.transform.position))
            {
                winner.gameObject.layer = LayerMask.NameToLayer("Non-Outline-Items");
                winner = closeItems[i];
            }
        }

        return winner;
    }

    public bool HasBodyArmor() { return (bodyArmor != null) ? true : false; }
}
