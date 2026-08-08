using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyEncounter : MonoBehaviour
{
    [System.Serializable]
    public struct EnemySpawn
    {
        public GameObject spawned;
        public Transform spawnpoint;
    }
    [System.Serializable]
    public struct EnemyWave
    {
        public List<EnemySpawn> spawnlist;
    }
    private int currentWave, aliveEnemies;
    private bool cleared = false, combat =  false, spawning = false;

    
    [SerializeField] private EnemyWave[] waves;

    private void OnTriggerEnter(Collider other)
    {
        if (combat)
            return;
        if (other.CompareTag("Player") && !cleared)
        {  
            combat = true;
            StartEncounter();
        }
    }

    public void StartEncounter()
    {
        currentWave = 0;
        StartCoroutine(NextWave(currentWave));
    }

    private IEnumerator NextWave(int currentWave)
    {


        Debug.Log(currentWave);

        if (currentWave >= waves.Length)
        {
            combat = false;
            cleared = true;
            yield break;
        }
        spawning = true;
        aliveEnemies = 0;

        foreach (EnemySpawn spawn in waves[currentWave].spawnlist) {
            if (spawn.spawned == null || spawn.spawnpoint == null)
                continue;
            PoEnemy enemySpawned = Instantiate(spawn.spawned, spawn.spawnpoint.position, spawn.spawnpoint.rotation).GetComponent<PoEnemy>();
            if (enemySpawned != null)
            {
                enemySpawned.OnDeath += RemoveEnemyInWave;
                aliveEnemies++;
            }
            yield return new WaitForSeconds(0.25f);
        }
        spawning = false;

    }

    public void RemoveEnemyInWave(PoEnemy removed)
    {
        aliveEnemies--;
        if (aliveEnemies <= 0 && !spawning)
        {
            currentWave++;
            StartCoroutine(NextWave(currentWave));
        }
    }
    
}
