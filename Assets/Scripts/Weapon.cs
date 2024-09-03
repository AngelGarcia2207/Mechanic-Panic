using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Buildable
{
   [SerializeField] protected int Damage;
   [SerializeField] protected WeaponBase weaponBase;
   [SerializeField] protected List<WeaponComplement> weaponComplements;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateMesh(MeshFilter newMesh, MeshRenderer newRenderer)
    {
        buildableMesh.mesh = newMesh.mesh;
        buildableRenderer.materials = newRenderer.materials;
    }
}
