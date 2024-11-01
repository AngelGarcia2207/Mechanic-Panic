using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Esta es la clase para el arma que lleva el jugador
*/

public class Obj_Player_Weapon : Obj_Buildable
{
    [SerializeField] protected int damage;
    [SerializeField] protected Obj_Weapon_Base weaponBase;
    [SerializeField] protected List<Obj_Weapon_Complement> weaponComplements;
    [SerializeField] protected GameObject complementPrefab;
    protected List<Vector3> currentPositions;
    protected Dictionary<ComplementLocations, int> numberOfElementsPerLocation = new();
    protected List<Obj_Weapon_Item> closeItems = new();
    protected List<ComplementLocations> closeItemsLocations = new();
    protected List<Vector3> closeItemsPositions = new();
    protected Obj_Weapon_Item closestItem;

    void Awake()
    {
        Mov_Player_Controller player = Finder.FindComponentInParents<Mov_Player_Controller>(transform);
        player.pickUpWeapon.AddListener(OnPickUp);
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
            foreach(Obj_Weapon_Item item in closeItems)
            {
                Debug.Log(item.gameObject.name);
            }
        }
    }

    //Add a base to this weapon
    public void SetBase(Obj_Weapon_Item newBase)
    {
        weaponBase = newBase.GetData() as Obj_Weapon_Base;
        buildableMesh.mesh = newBase.GetMesh().mesh;
        buildableRenderer.materials = newBase.GetRenderer().materials;
        currentPositions = weaponBase.GetInitialPositions(numberOfElementsPerLocation);
        damage = weaponBase.GetDamage();
        Collider weaponCollider = gameObject.AddComponent(newBase.GetCollider().GetType()) as Collider;
        if(weaponCollider is BoxCollider boxCollider)
        {
            BoxCollider newBoxCollider = newBase.GetCollider() as BoxCollider;
            boxCollider.center = newBoxCollider.center;
            boxCollider.size = newBoxCollider.size;
            boxCollider.isTrigger = true;
        }
    }

    //Add a complement to this weapon
    public void AddComplement(Obj_Weapon_Item newComplement)
    {
        //Initial data
        weaponComplements.Add(newComplement.GetData() as Obj_Weapon_Complement);
        numberOfElementsPerLocation[closeItemsLocations[closeItems.IndexOf(newComplement)]]++;
        /*foreach(Obj_Weapon_Item item in closeItems)
        {
            Debug.Log(item);
        }
        foreach(ComplementLocations item in closeItemsLocations)
        {
            Debug.Log(item);
        }*/
        int locationIndex = weaponBase.GetLocationIndex(closeItemsLocations[closeItems.IndexOf(newComplement)]);

        //Make instance of complement nad position it
        GameObject instantiatedComplement = Instantiate(complementPrefab, transform.position, new Quaternion(0, 0, 0, 0), this.transform);
        instantiatedComplement.transform.localPosition = closeItemsPositions[closeItems.IndexOf(newComplement)];
        instantiatedComplement.transform.Rotate(weaponBase.GetAngleChange(locationIndex));
        
        //Update positions
        currentPositions[locationIndex] = closeItemsPositions[closeItems.IndexOf(newComplement)];
        Vector3 temp = currentPositions[locationIndex];
        switch(closeItemsLocations[closeItems.IndexOf(newComplement)])
        {
            case ComplementLocations.Front:
                temp.z -= newComplement.GetMesh().mesh.bounds.size.z/2;
                break;
                
            case ComplementLocations.Right:
                temp.x += newComplement.GetMesh().mesh.bounds.size.z/2;
                break;
                
            case ComplementLocations.Left:
                temp.x -= newComplement.GetMesh().mesh.bounds.size.z/2;
                break;
                
            case ComplementLocations.Back:
                temp.z += newComplement.GetMesh().mesh.bounds.size.z/2;
                break;

            default:
                Debug.Log("Error");
                break;
        }
        temp.y += newComplement.GetMesh().mesh.bounds.size.y/2;
        currentPositions[locationIndex] = temp;

        //Render complement
        instantiatedComplement.GetComponent<MeshFilter>().mesh =  newComplement.GetMesh().mesh;
        instantiatedComplement.GetComponent<MeshRenderer>().materials = newComplement.GetRenderer().materials;

        //Add collisions
        Collider weaponCollider = instantiatedComplement.AddComponent(newComplement.GetCollider().GetType()) as Collider;
        if(weaponCollider is BoxCollider boxCollider)
        {
            BoxCollider newBoxCollider = newComplement.GetCollider() as BoxCollider;
            boxCollider.center = newBoxCollider.center;
            boxCollider.size = newBoxCollider.size;
            boxCollider.isTrigger = true;
        }
        else if(weaponCollider is SphereCollider sphereCollider)
        {
            SphereCollider newSphereCollider = newComplement.GetCollider() as SphereCollider;
            sphereCollider.center = newSphereCollider.center;
            sphereCollider.radius = newSphereCollider.radius;
            sphereCollider.isTrigger = true;
        }
        else if(weaponCollider is MeshCollider meshCollider)
        {
            MeshCollider newMeshCollider = newComplement.GetCollider() as MeshCollider;
            meshCollider.convex = newMeshCollider.convex;
            meshCollider.sharedMesh = newMeshCollider.sharedMesh;
            meshCollider.isTrigger = true;
        }

        //ReloadCloseItems
        closeItemsLocations.RemoveAt(closeItems.IndexOf(closestItem));
        closeItemsPositions.RemoveAt(closeItems.IndexOf(closestItem));
        closeItems.Remove(closestItem);
        List<Obj_Weapon_Item> tempCloseItems = closeItems;
        closeItems = new List<Obj_Weapon_Item>();
        foreach(Obj_Weapon_Item closeItem in tempCloseItems)
        {
            closeItem.gameObject.layer = LayerMask.NameToLayer("Non-Outline-Items");
            if(CheckIfFits(closeItem))
            {
                AddCloseItem(closeItem);
            }
        }
    }

    public void OnPickUp(Obj_Player_Weapon playerWeapon)
    {
        if(closeItems.Count == 0)
        {
            return;
        }

        if(closestItem.GetData() is Obj_Weapon_Base)
        {
            SetBase(closestItem);
            closeItems.Remove(closestItem);
        }
        else
        {
            AddComplement(closestItem);
        }

        closestItem.DestroyThis();
        closestItem = null;
    }

    public int DealDamage(int complementIndex = -1)
    {
        if(complementIndex < 0)
        {
            return damage;
        }
        else
        {
            return weaponComplements[complementIndex - 2].GetDamage();
        }
    }

    public void AddCloseItem(Obj_Weapon_Item newItem)
    {
        closeItems.Add(newItem);
    }

    public void RemoveCloseItem(Obj_Weapon_Item exitItem)
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
        
        if(exitItem.GetData() is Obj_Weapon_Complement)
        {
            closeItemsLocations.RemoveAt(closeItems.IndexOf(exitItem));
            closeItemsPositions.RemoveAt(closeItems.IndexOf(exitItem));
        }
        
        closeItems.Remove(exitItem);
    }

    public Obj_Weapon_Item GetClosestItem()
    {   
        Obj_Weapon_Item winner = closeItems[0];
        Vector3 playerPosition = transform.parent.gameObject.transform.parent.gameObject.transform.position;

        /*foreach(Obj_Weapon_Item closeItem in closeItems)
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

    public bool CheckIfFits(Obj_Weapon_Item complementItem)
    {
        Obj_Weapon_Complement complementData = complementItem.GetData() as Obj_Weapon_Complement;
        bool locationFound = false;
        ComplementLocations bestLocation;
        List<ComplementLocations> availableLocations = new();
        foreach(ComplementLocations location in complementData.GetLocationsList())
        {
            availableLocations.Add(location);
        }
        Vector3 tempPosition;

        for(int i = 0; i < complementData.GetLocationsList().Count; i++)
        {
            bestLocation = Obj_Weapon_Complement.GetBestLocation(numberOfElementsPerLocation, availableLocations);
            if(bestLocation == ComplementLocations.Center)
            {
                break;
            }
            availableLocations.Remove(bestLocation);

            tempPosition = weaponBase.GetNewComplementPosition(bestLocation, currentPositions[weaponBase.GetLocationIndex(bestLocation)],
                complementItem.GetMesh().mesh.bounds.size);
            
            if(tempPosition != new Vector3(0, 0, 0))
            {
                locationFound = true;
                closeItemsLocations.Add(bestLocation);
                closeItemsPositions.Add(tempPosition);
                break;
            }
        }

        return locationFound;
    }

    public bool HasBase() { return (weaponBase != null) ? true : false; }
}