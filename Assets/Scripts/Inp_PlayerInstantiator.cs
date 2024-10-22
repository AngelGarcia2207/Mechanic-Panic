using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inp_PlayerInstantiator : MonoBehaviour
{
    [SerializeField] private GameObject[] characterObj;


    void Start()
    {
        Map_Display_Boundaries.Instance.AddPlayer(this.gameObject);
    }

    
}
