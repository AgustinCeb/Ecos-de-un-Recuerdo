using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Enemy_Camp : NetworkBehaviour
{
    [SerializeField] private List<Enemy_Spawn> _enemySpawns = new();

    private void Awake()
    {
        _enemySpawns.Clear();
        _enemySpawns.AddRange(GetComponentsInChildren<Enemy_Spawn>());
    }


    public void SpawnEnemies()
    {
        if (!IsServer) return;

        foreach (Enemy_Spawn spawn in _enemySpawns)
        {
            if (spawn.IsDefeated()) continue;
            if (spawn.HasSpawnedEnemy()) continue;

            GameObject prefab = spawn.GetEnemyPrefab();

            if (prefab == null)
            {
                Debug.LogWarning("EnemySpawn no tiene prefab asignado.");
                continue;
            }

            GameObject enemy = Instantiate(
                prefab,
                spawn.GetSpawnPosition(),
                spawn.GetSpawnRotation()
            );

            NetworkObject networkObject =
                enemy.GetComponent<NetworkObject>();

            if (networkObject == null)
            {
                Debug.LogError("El enemigo no tiene NetworkObject.");
                Destroy(enemy);
                continue;
            }

            EnemiesHealt health = enemy.GetComponent<EnemiesHealt>();

            if (health == null)
            {
                Debug.LogError("El enemigo no tiene EnemiesHealt.");
                Destroy(enemy);
                continue;
            }

            health.SetEnemySpawn(spawn);

            spawn.SetSpawnedEnemy(enemy);

            networkObject.Spawn();
        }
    }

    public void DespawnEnemies()
    {
        if (!IsServer) return;
        
        foreach (Enemy_Spawn spawn in _enemySpawns)
        {
            GameObject enemy = spawn.GetSpawnedEnemy();

            if (enemy == null) continue;

            NetworkObject networkObject =
                enemy.GetComponent<NetworkObject>();

            if (networkObject != null && networkObject.IsSpawned)
            {
                networkObject.Despawn();
            }

            spawn.SetSpawnedEnemy(null);
        }
    }

    public void ResetCamp()
    {
        if (!IsServer) return;

        DespawnEnemies();

        foreach (Enemy_Spawn spawn in _enemySpawns)
        {
            spawn.ResetSpawn();
        }
    }

}
