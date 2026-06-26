using UnityEngine;
using Unity.Netcode;

public class EnemiesHealt : NetworkBehaviour
{
    
    public NetworkVariable <int> EnemyHealt = new(50);
    [SerializeField] GameObject expOrb;

    public void TakeDamage(int damage)
    {
        if (!IsServer) return;

        EnemyHealt.Value -= damage;

        if (EnemyHealt.Value <= 0)
        {
            GameObject orb = Instantiate(expOrb,this.transform.position,Quaternion.identity);
            orb.GetComponent<NetworkObject>().Spawn();
            NetworkObject.Despawn();
        }

    }

}
