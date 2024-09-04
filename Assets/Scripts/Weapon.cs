using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Buildable
{
   [SerializeField] protected int Damage;
   [SerializeField] protected WeaponBase weaponBase;
   [SerializeField] protected List<WeaponComplement> weaponComplements;
   [SerializeField] protected GameObject complementPrefab;
   
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(complementPosition);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBase(WeaponBase newBase, MeshFilter newMesh, MeshRenderer newRenderer)
    {
        weaponBase = newBase;
        buildableMesh.mesh = newMesh.mesh;
        buildableRenderer.materials = newRenderer.materials;
    }

    public void AddComplement(WeaponComplement newComplement, MeshFilter newMesh, MeshRenderer newRenderer)
    {
        weaponComplements.Add(newComplement);
        GameObject instantiatedComplement = Instantiate(complementPrefab, complementPosition, Quaternion.identity, this.gameObject.GetComponent<Transform>());
        instantiatedComplement.GetComponent<MeshFilter>().mesh = newMesh.mesh;
        instantiatedComplement.GetComponent<MeshRenderer>().materials = newRenderer.materials;
    }
}
