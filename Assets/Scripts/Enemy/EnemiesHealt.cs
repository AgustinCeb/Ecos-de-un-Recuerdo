using UnityEngine;
using Unity.Netcode;

public class EnemiesHealt : NetworkBehaviour
{
    
    public NetworkVariable <int> EnemyHealt = new(50);

    public void TakeDamage(int damage)
    {
        if (!IsServer) return;

        EnemyHealt.Value -= damage;

        if (EnemyHealt.Value <= 0)
        {
            NetworkObject.Despawn();

        }

    }

}
