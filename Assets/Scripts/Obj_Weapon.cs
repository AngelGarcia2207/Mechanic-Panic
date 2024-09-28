using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Esta es la clase para el arma que lleva el jugador
*/

public class Obj_Weapon : Obj_Buildable
{
   [SerializeField] protected int damage;
   [SerializeField] protected Obj_Weapon_Base weaponBase;
   [SerializeField] protected List<Obj_Weapon_Complement> weaponComplements;
   [SerializeField] protected GameObject complementPrefab;
   protected List<Vector3> currentPositions;
   protected Dictionary<ComplementPositions, int> elementsInPosition = new();
   
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(complementPosition);
    }

    // Update is called once per frame
    void Update()
    {
        //
    }

    //Añadir base al arma
    public void SetBase(Obj_Weapon_Base newBase, MeshFilter newMesh, MeshRenderer newRenderer, Collider newCollider)
    {
        weaponBase = newBase;
        buildableMesh.mesh = newMesh.mesh;
        buildableRenderer.materials = newRenderer.materials;
        currentPositions = newBase.GetInitialPositions(elementsInPosition);
        damage = newBase.GetDamage();
        Collider weaponCollider = gameObject.AddComponent(newCollider.GetType()) as Collider;
        if(weaponCollider is BoxCollider boxCollider)
        {
            BoxCollider newBoxCollider = newCollider as BoxCollider;
            boxCollider.center = newBoxCollider.center;
            boxCollider.size = newBoxCollider.size;
        }
    }

    //Añadir un complemento al arma
    public void AddComplement(Obj_Weapon_Complement newComplement, MeshFilter newFilter, MeshRenderer newRenderer, Collider newCollider)
    {
        //Datos iniciales
        weaponComplements.Add(newComplement);
        ComplementPositions bestPosition = newComplement.GetBestPosition(elementsInPosition);
        int positionIndex = weaponBase.GetPositionIndex(bestPosition);

        //Instanciar y posicionar complemento
        GameObject instantiatedComplement = Instantiate(complementPrefab, this.gameObject.GetComponent<Transform>().position,
        new Quaternion(0, 0, 0, 0), this.gameObject.GetComponent<Transform>());
        instantiatedComplement.GetComponent<Transform>().localPosition = weaponBase.GetNewComplementPosition(
            bestPosition, currentPositions[positionIndex], newFilter.mesh.bounds.size);
        instantiatedComplement.GetComponent<Transform>().Rotate(weaponBase.GetAngleChange(positionIndex));
        
        //Actualizar posiciones
        currentPositions[positionIndex] = instantiatedComplement.GetComponent<Transform>().localPosition;
        Vector3 temp = currentPositions[positionIndex];
        switch(bestPosition)
        {
            case ComplementPositions.Front:
                temp.z += newFilter.mesh.bounds.size.z/2;
                break;
                
            case ComplementPositions.Right:
                temp.x -= newFilter.mesh.bounds.size.z/2;
                break;
                
            case ComplementPositions.Left:
                temp.x += newFilter.mesh.bounds.size.z/2;
                break;
                
            case ComplementPositions.Back:
                temp.z -= newFilter.mesh.bounds.size.z/2;
                break;

            default:
                Debug.Log("Error");
                break;
        }
        temp.y += newFilter.mesh.bounds.size.y/2;
        currentPositions[positionIndex] = temp;

        //Renderizar complemento
        instantiatedComplement.GetComponent<MeshFilter>().mesh = newFilter.mesh;
        instantiatedComplement.GetComponent<MeshRenderer>().materials = newRenderer.materials;

        //Añadir colisiones
        Collider weaponCollider = instantiatedComplement.AddComponent(newCollider.GetType()) as Collider;
        if(weaponCollider is BoxCollider boxCollider)
        {
            BoxCollider newBoxCollider = newCollider as BoxCollider;
            boxCollider.center = newBoxCollider.center;
            boxCollider.size = newBoxCollider.size;
        }
        else if(weaponCollider is SphereCollider sphereCollider)
        {
            SphereCollider newSphereCollider = newCollider as SphereCollider;
            sphereCollider.center = newSphereCollider.center;
            sphereCollider.radius = newSphereCollider.radius;
        }
        else if(weaponCollider is MeshCollider meshCollider)
        {
            MeshCollider newMeshCollider = newCollider as MeshCollider;
            meshCollider.convex = newMeshCollider.convex;
            meshCollider.sharedMesh = newMeshCollider.sharedMesh;
        }
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
}