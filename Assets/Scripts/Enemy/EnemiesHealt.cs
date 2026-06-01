using UnityEngine;
using Unity.Netcode;

public class EnemiesHealt : NetworkBehaviour
{

    public int _enemyHealt = 5;

    public void TakeDamage(int damage)
    {
        _enemyHealt -= damage;
        if (_enemyHealt <= 0)
        {
            if (IsServer)
            {
                NetworkObject.Despawn();
            }

        }
    }

}
