using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Esta es la clase para el arma que lleva el jugador
*/

public class Obj_Player_Weapon : Obj_Buildable
{
    [SerializeField] protected int damage;
    [SerializeField] protected int currentDurability;
    [SerializeField] protected Obj_Weapon_Base weaponBase;
    [SerializeField] protected GameObject basePrefab;
    [SerializeField] protected List<Obj_Weapon_Complement> weaponComplements;
    [SerializeField] protected GameObject complementPrefab;
    protected SFX_Player_AudioClips audioClips;
    protected List<Vector3> currentPositions;
    protected Dictionary<ComplementLocations, int> numberOfElementsPerLocation = new();
    protected List<Obj_Weapon_Item> closeItems = new();
    protected List<ComplementLocations> closeItemsLocations = new();
    protected List<Vector3> closeItemsPositions = new();
    protected Obj_Weapon_Item closestItem;
    private List<GameObject> instantiatedComplements = new List<GameObject>();

    void Awake()
    {
        currentDurability = weaponBase != null ? weaponBase.GetDurability() : 0;
        Mov_Player_Controller player = Finder.FindComponentInParents<Mov_Player_Controller>(transform);
        player.pickUpWeapon.AddListener(OnPickUp);
        audioClips = player.audioClips;
    }
    
    void Update()
    {
        closeItems.RemoveAll(item => item == null);

        if (closeItems.Count == 0)
        {
            return;
        }

        // Encuentra el objeto más cercano
        closestItem = GetClosestItem();

        if(closestItem != null && closestItem.isPlayer == false && closestItem.gameObject.layer != LayerMask.NameToLayer("Outline-Items"))
        {
            closestItem.gameObject.layer = LayerMask.NameToLayer("Outline-Items");
        }
    }

    //Add a base to this weapon
    public void SetBase(Obj_Weapon_Item newBase)
    {
        weaponBase = newBase.GetData() as Obj_Weapon_Base;
        currentDurability = weaponBase.GetDurability();
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

    public void DeleteWeapon()
    {
        weaponBase = null;
        currentDurability = 0;

        Destroy(buildableMesh.mesh);

        currentPositions.Clear();
        damage = 0;

        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider collider in colliders)
        {
            Destroy(collider);
        }

        foreach (GameObject complement in instantiatedComplements)
        {
            Destroy(complement);
        }
        instantiatedComplements.Clear();
        weaponComplements.Clear();

        // Reinicializa el diccionario con todas las claves posibles
        numberOfElementsPerLocation.Clear();
        foreach (ComplementLocations location in System.Enum.GetValues(typeof(ComplementLocations)))
        {
            numberOfElementsPerLocation[location] = 0;
        }
    }

    //Add a player as a weapon
    public void SetPlayerAsBase(Obj_Weapon_Item newBase)
    {
        weaponBase = newBase.GetData() as Obj_Weapon_Base;
        currentDurability = weaponBase.GetDurability();
        damage = weaponBase.GetDamage();
        newBase.transform.SetParent(transform);
        newBase.transform.localPosition = new Vector3(0.01f, 0, 0.04f);
        newBase.transform.localEulerAngles = new Vector3(160, 0, 0);
        newBase.gameObject.GetComponent<Mov_Player_Controller>().enabled = false;
        newBase.gameObject.GetComponent<CharacterController>().enabled = false;
    }

    //Add a complement to this weapon
    public void AddComplement(Obj_Weapon_Item newComplement)
    {

        weaponComplements.Add(newComplement.GetData() as Obj_Weapon_Complement);
        numberOfElementsPerLocation[closeItemsLocations[closeItems.IndexOf(newComplement)]]++;

        int locationIndex = weaponBase.GetLocationIndex(closeItemsLocations[closeItems.IndexOf(newComplement)]);

        // Crear instancia y posicionarla
        GameObject instantiatedComplement = Instantiate(complementPrefab, transform.position, new Quaternion(0, 0, 0, 0), this.transform);
        instantiatedComplements.Add(instantiatedComplement);
        instantiatedComplement.transform.localPosition = closeItemsPositions[closeItems.IndexOf(newComplement)];
        instantiatedComplement.transform.Rotate(weaponBase.GetAngleChange(locationIndex));

        // Actualizar posiciones y añadir colisiones
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
            if(closeItem.isPlayer == false)
            {
                closeItem.gameObject.layer = LayerMask.NameToLayer("Non-Outline-Items");
            }
            
            if(CheckIfFits(closeItem))
            {
                AddCloseItem(closeItem);
            }
        }
    }

    public void OnPickUp(Obj_Player_Weapon playerWeapon)
    {
        if (closeItems.Count == 0)
        {
            return;
        }

        foreach(Obj_Weapon_Item item in closeItems)
        {
            Debug.Log(item.gameObject.name);
        }

        if(closestItem.GetData() is Obj_Weapon_Base)
        {
            Debug.Log(playerWeapon.transform.parent.parent.parent.parent.parent.parent.parent);
            if(closestItem.isPlayer)
            {
                SetPlayerAsBase(closestItem);
            }
            else
            {
                SetBase(closestItem);
                basePrefab = closestItem.gameObject;
                basePrefab.GetComponent<Rigidbody>().isKinematic = true;
                basePrefab.SetActive(false);
                basePrefab.transform.SetParent(null);
                closeItems.Remove(closestItem);
            }
        }
        else
        {
            AddComplement(closestItem);
            closestItem.DestroyThis();
        }

        audioClips.itemPickupAudio();
        closestItem = null;
    }

    public void ThrowWeapon()
    {
        if(weaponBase == null)
        {
            return;
        }

        List<Transform> complementsToReparent = new List<Transform>();

        for (int i = 2; i < transform.childCount; i++)
        {
            complementsToReparent.Add(transform.GetChild(i));
        }

        foreach (Transform complement in complementsToReparent)
        {
            complement.SetParent(basePrefab.transform, false);
        }

        basePrefab.transform.position = transform.position;
        basePrefab.transform.rotation = transform.rotation;
        basePrefab.SetActive(true);
        Rigidbody rb = basePrefab.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = basePrefab.AddComponent<Rigidbody>();
        }
        rb.isKinematic = false;
        rb.AddForce(-transform.forward * 400 + new Vector3(0, 250, 0));


        weaponBase = null;
        currentDurability = 0;
        buildableMesh.mesh = null;
        buildableRenderer.materials = new Material[0];
        damage = 0;
        Destroy(gameObject.GetComponent<BoxCollider>());
    }

    public int DealDamage(int complementIndex = -1)
    {
        if (currentDurability > 0)
        {
            int damageToDeal = complementIndex < 0 ? damage : weaponComplements[complementIndex - 2].GetDamage();
            currentDurability --;

            if (currentDurability <= 0)
            {
                DeleteWeapon();
            }

            return damageToDeal;
        }
        else
        {
            return 10;
        }
    }

    public void AddCloseItem(Obj_Weapon_Item newItem)
    {
        if(newItem.gameObject == transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.gameObject)
        {
            return;
        }
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
            if(closestItem.isPlayer == false)
            {
                closestItem.gameObject.layer = LayerMask.NameToLayer("Non-Outline-Items");
            }
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
        if (closeItems.Count == 0)
        {
            return null; // No hay elementos cercanos
        }

        Obj_Weapon_Item winner = closeItems[0];
        float closestDistance = float.MaxValue;
        Vector3 playerPosition = transform.parent.gameObject.transform.parent.gameObject.transform.position;

        foreach (Obj_Weapon_Item item in closeItems)
        {
            if (item == null)
            {
                Debug.LogWarning("Se encontró un objeto destruido en closeItems. Eliminándolo de la lista.");
                continue;
            }

            float distance = Vector3.Distance(playerPosition, item.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                winner = item;
                if(winner.isPlayer == false)
                {
                    winner.gameObject.layer = LayerMask.NameToLayer("Non-Outline-Items");
                }
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

    public void ActivateEffects(int itemIndex, Transform target)
    {
       int complementIndex = itemIndex - 2;
        if (complementIndex < 0 || complementIndex >= weaponComplements.Count)
        {
            return;
        }

        // Ejecutar el efecto si el índice es válido
        weaponComplements[complementIndex].PlayEffects(target);
    }
}