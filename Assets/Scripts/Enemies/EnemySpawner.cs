using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyQueue;
    [SerializeField] private Transform[] fixedSpawnPoints;
    [SerializeField] private int maxActiveEnemies;
    [SerializeField] private float spawnCooldown;
    private int activeEnemies;
    private bool readyToSpawn = false;
    
    void Update()
    {
        if(readyToSpawn)
        {
            GameObject newEnemy = Instantiate(enemyQueue[0], fixedSpawnPoints[Random.Range(0, fixedSpawnPoints.Length)].position, Quaternion.identity);
            newEnemy.GetComponent<Ene_EnemyTest>().death.AddListener(OnEnemyDeath);
            enemyQueue.RemoveAt(0);
            activeEnemies++;
            readyToSpawn = false;

            if(enemyQueue.Count > 0 && activeEnemies <= maxActiveEnemies)
            {
                StartCoroutine(SpawnCooldown());
            }
        }

        if(activeEnemies == 0 && enemyQueue.Count == 0)
        {
            Map_Display_Boundaries.Instance.ToggleMovementLock(false);
            Debug.Log("Enemigos derrotados");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //iniciar pelea, quitar follow de camara
            StartCoroutine(SpawnCooldown());
            Destroy(gameObject.GetComponent<Collider>());
            Map_Display_Boundaries.Instance.ToggleMovementLock(true);
        }
    }

    private IEnumerator SpawnCooldown()
    {
        yield return new WaitForSeconds(spawnCooldown);
        readyToSpawn = true;
    }

    public void OnEnemyDeath()
    {
        activeEnemies--;
    }
}
