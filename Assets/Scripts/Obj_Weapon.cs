using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Esta es la clase para el arma que lleva el jugador
*/

public class Obj_Weapon : Obj_Buildable
{
   [SerializeField] protected int Damage;
   [SerializeField] protected Obj_Weapon_Base weaponBase;
   [SerializeField] protected List<Obj_Weapon_Complement> weaponComplements;
   [SerializeField] protected GameObject complementPrefab;
   
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
    public void SetBase(Obj_Weapon_Base newBase, MeshFilter newMesh, MeshRenderer newRenderer)
    {
        weaponBase = newBase;
        complementPosition = weaponBase.GetComplementPosition();
        positionChange = weaponBase.GetPositionChange();
        buildableMesh.mesh = newMesh.mesh;
        buildableRenderer.materials = newRenderer.materials;
    }

    //Añadir un complemento al arma
    public void AddComplement(Obj_Weapon_Complement newComplement, MeshFilter newMesh, MeshRenderer newRenderer)
    {
        weaponComplements.Add(newComplement);
        GameObject instantiatedComplement = Instantiate(complementPrefab, this.gameObject.GetComponent<Transform>().position,
        Quaternion.identity, this.gameObject.GetComponent<Transform>());
        Debug.Log(complementPosition);
        instantiatedComplement.GetComponent<Transform>().localPosition = complementPosition;
        instantiatedComplement.GetComponent<MeshFilter>().mesh = newMesh.mesh;
        instantiatedComplement.GetComponent<MeshRenderer>().materials = newRenderer.materials;
    }
}