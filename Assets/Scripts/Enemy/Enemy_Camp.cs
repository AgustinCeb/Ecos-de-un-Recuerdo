using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Enemy_Camp : NetworkBehaviour
{
    [SerializeField] private List<Enemy_Spawn> _enemySpawns = new List<Enemy_Spawn>();

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
            if (spawn.IsAlive()) continue;

            GameObject prefab = spawn.GetEnemyPrefab();

            if (prefab == null)
            {
                Debug.LogWarning("EnemySpawn no tiene un prefab asignado.");
                continue;
            }

            GameObject enemy = Instantiate(
                prefab,
                spawn.GetSpawnPosition(),
                spawn.GetSpawnRotation()
            );

            NetworkObject networkObject = enemy.GetComponent<NetworkObject>();

            if (networkObject == null)
            {
                Debug.LogError("El enemigo no tiene NetworkObject.");
                Destroy(enemy);
                continue;
            }

            networkObject.Spawn();

            spawn.SetSpawnedEnemy(enemy);
        }
    }

    public void DespawnEnemies()
    {
        if (!IsServer) return;

        foreach (Enemy_Spawn spawn in _enemySpawns)
        {
            GameObject enemy = spawn.GetSpawnedEnemy();

            if (enemy == null) continue;

            NetworkObject networkObject = enemy.GetComponent<NetworkObject>();

            if (networkObject != null && networkObject.IsSpawned)
            {
                networkObject.Despawn();
            }

            spawn.SetSpawnedEnemy(null);
        }
    }

}
