using Unity.Netcode;
using UnityEngine;

public class Onl_Miner : NetworkBehaviour
{
    [HideInInspector] public Miner miner;
    [HideInInspector] public Ene_EnemyTest enemyTest;

    public NetworkVariable<int> currentHealthOnl = new NetworkVariable<int>();

    void Start()
    {
        miner = GetComponent<Miner>();
        enemyTest = GetComponent<Ene_EnemyTest>();

        if (IsClient)
        {
            currentHealthOnl.OnValueChanged += OnHealthChanged;
        }
    }

    // HEALTH
    public void SetHealth(int health)
    {
        if (IsServer)
        {
            currentHealthOnl.Value = health;
        }
    }

    private void OnHealthChanged(int oldValue, int newValue)
    {
        if (newValue <= 0)
        {
            Destroy(this.gameObject);
        }
        enemyTest.UpdateHealth(newValue);
    }

    // Destroy the miner across all clients
    [ServerRpc(RequireOwnership = false)]
    public void DestroyMinerServerRpc()
    {
        DestroyMiner();
    }

    public void DestroyMiner()
    {
        if (IsServer)
        {
            Debug.Log("Destroying miner on server.");
            NetworkObject.Despawn(true); // Despawn the networked object and destroy the GameObject
        }
    }
}
