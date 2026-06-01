using UnityEngine;
using Unity.Netcode;

public class PlayerHealt : NetworkBehaviour
{
    public NetworkVariable <int> Healt = new(100);

    public void TakeDamagePlayer(int enemyDamage)
    {
        if(!IsServer) return;

        Healt.Value -= enemyDamage;

        if(Healt.Value <= 0)
        {
            NetworkObject.Despawn();
        
        }


    }
}
