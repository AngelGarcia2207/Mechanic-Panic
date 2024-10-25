using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyQueue;
    [SerializeField] private Transform[] fixedSpawnPoints;
    [SerializeField] private Cam_Default_Controller camera;
    [SerializeField] private int maxActiveEnemies;
    [SerializeField] private float spawnCooldown;
    private int activeEnemies;
    private bool readyToSpawn = false;
    
    void Update()
    {
        if(readyToSpawn)
        {
            GameObject newEnemy = Instantiate(enemyQueue[0], fixedSpawnPoints[Random.Range(0, fixedSpawnPoints.Length)].position, Quaternion.identity);
            enemyQueue.RemoveAt(0);
            if(newEnemy.transform.GetChild(1).gameObject.TryGetComponent<IAs_SmallBot_SM>(out IAs_SmallBot_SM smallSM))
            {
                smallSM.UpdateCenter(new Vector3(camera.transform.position.x, 0, 0));
            }
            activeEnemies++;
            readyToSpawn = false;

            if(enemyQueue.Count > 0 && activeEnemies <= maxActiveEnemies)
            {
                StartCoroutine(SpawnCooldown());
            }
        }

        if(activeEnemies == 0 && enemyQueue.Count == 0)
        {
            //terminar pelea, devolver follow de camara
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //iniciar pelea, quitar follow de camara
            StartCoroutine(SpawnCooldown());
            Destroy(gameObject.GetComponent<Collider>());
        }
    }

    private IEnumerator SpawnCooldown()
    {
        yield return new WaitForSeconds(spawnCooldown);
        readyToSpawn = true;
    }
}
