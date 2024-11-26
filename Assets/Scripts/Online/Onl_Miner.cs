using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Onl_Miner : NetworkBehaviour
{
    [HideInInspector] public Miner miner;
    [HideInInspector] public Ene_EnemyTest enemyTest;


    void Start()
    {
        miner = GetComponent<Miner>();
        enemyTest = GetComponent<Ene_EnemyTest>();
    }
}
