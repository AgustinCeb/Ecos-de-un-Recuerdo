using UnityEngine;

public class Enemy_Spawn : MonoBehaviour
{
    [Header("Enemy Configuration")]
    [SerializeField] private GameObject _enemyPrefab;

    private GameObject _spawnedEnemy;

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

    public bool IsAlive()
    {
        return _spawnedEnemy != null;
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
