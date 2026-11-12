using UnityEngine;

public class Enemy_Spawn : MonoBehaviour
{

    [Header("Enemy Configuration")]
    [SerializeField] private GameObject _enemyPrefab;

    private GameObject _spawnedEnemy;
    private bool _enemyDefeated;

    public GameObject GetEnemyPrefab()
    {
        return _enemyPrefab;
    }

    public GameObject GetSpawnedEnemy()
    {
        return _spawnedEnemy;
    }

    public void SetSpawnedEnemy(GameObject enemy)
    {
        _spawnedEnemy = enemy;
    }

    public bool IsDefeated()
    {
        return _enemyDefeated;
    }

    public bool HasSpawnedEnemy()
    {
        return _spawnedEnemy != null;
    }

    public void RegisterDeath()
    {
        _enemyDefeated = true;
        _spawnedEnemy = null;
    }

    public void ResetSpawn()
    {
        _enemyDefeated = false;
    }

    public Vector3 GetSpawnPosition()
    {
        return transform.position;
    }

    public Quaternion GetSpawnRotation()
    {
        return transform.rotation;
    }

}
