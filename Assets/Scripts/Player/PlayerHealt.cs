using UnityEngine;
using Unity.Netcode;

public class PlayerHealt : NetworkBehaviour
{
    public int _healt = 100;

    public void TakeDamagePlayer(int enemyDamage)
    {
        _healt -= enemyDamage;
        if (_healt < 0)
        {
            if(IsServer)
            {
                NetworkObject.Despawn();
            }
        }


    }
}
