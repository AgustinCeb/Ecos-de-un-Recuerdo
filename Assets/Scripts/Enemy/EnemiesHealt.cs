using UnityEngine;
using Unity.Netcode;

public class EnemiesHealt : NetworkBehaviour
{
    
    public NetworkVariable <int> EnemyHealt = new(50);
    [SerializeField] GameObject expOrb;
    [SerializeField] GameObject itemDroop;

    private Enemy_Spawn _enemySpawn;

    public void SetEnemySpawn(Enemy_Spawn spawn)
    {
        _enemySpawn = spawn;
    }

    public void TakeDamage(int damage)
    {
        if (!IsServer) return;

        EnemyHealt.Value -= damage;

        if (EnemyHealt.Value <= 0)
        {
            if (_enemySpawn != null)
            {
                _enemySpawn.RegisterDeath();
            }

            GameObject orb = Instantiate(expOrb,this.transform.position,Quaternion.identity);

            GameObject droop = Instantiate(itemDroop, this.transform.position, Quaternion.identity);

            orb.GetComponent<NetworkObject>().Spawn();

            droop.GetComponent<NetworkObject>().Spawn();

            NetworkObject.Despawn();
        }

    }
}
