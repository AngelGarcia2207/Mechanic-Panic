using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Esta clase es la plantilla para crear items de armadura.
*/

public abstract class Obj_Armor_Component : ScriptableObject
{
    [SerializeField] protected int Defense;
    [SerializeField] protected int Weight;
    [SerializeField] protected List<ArmorEffect> effects;
    [SerializeField] protected GameObject itemPrefab;
    
    [SerializeField] Vector3 position;
    [SerializeField] Vector3 eulerRotation;
    [SerializeField] Vector3 scale;

    public int GetDefense() { return Defense; }
    public GameObject GetPrefab() { return itemPrefab; }
    public void UpdateMeshTransform(Transform mesh, Vector3 newPosition, Vector3 newScale)
    {
        mesh.localPosition = newPosition;
        mesh.localEulerAngles = eulerRotation;
        mesh.localScale = newScale;
    }
}