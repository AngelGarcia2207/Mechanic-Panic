using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyQueue;
    [SerializeField] private Transform[] fixedSpawnPoints;
    [SerializeField] private Transform miniBossSpawn;
    [SerializeField] private int maxActiveEnemies;
    [SerializeField] private float spawnCooldown;
    [SerializeField] private int activeEnemies = 0;
    private bool readyToSpawn = false;
    
    void Update()
    {
        if(activeEnemies == 0 && enemyQueue.Count == 0)
        {
            Map_Display_Boundaries.Instance.ToggleMovementLock(false);
            Destroy(gameObject);
            return;
        }
        
        if(readyToSpawn && enemyQueue.Count > 0)
        {
            GameObject newEnemy = Instantiate(enemyQueue[0]);
            newEnemy.GetComponent<Ene_EnemyTest>().death.AddListener(OnEnemyDeath);
            if(newEnemy.transform.GetChild(1).gameObject.TryGetComponent(out IAs_LogLauncher_SM comp))
            {
                newEnemy.transform.position = miniBossSpawn.position;
            }
            else
            {
                newEnemy.transform.position = fixedSpawnPoints[Random.Range(0, fixedSpawnPoints.Length)].position;
            }
            enemyQueue.RemoveAt(0);
            activeEnemies++;
            readyToSpawn = false;

            if(enemyQueue.Count > 0 && activeEnemies < maxActiveEnemies)
            {
                StartCoroutine(SpawnCooldown());
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("CameraCenter"))
        {
            Map_Display_Boundaries.Instance.ToggleMovementLock(true);
            StartCoroutine(SpawnCooldown());
            Destroy(gameObject.GetComponent<Collider>());
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
        if(activeEnemies < maxActiveEnemies && enemyQueue.Count > 0)
        {
            StartCoroutine(SpawnCooldown());
        }
    }
}
