using UnityEngine;
using Unity.Netcode;

public class PlayerHealt : NetworkBehaviour
{
    public NetworkVariable <int> Healt = new(100);

    public NetworkVariable<int> ShieldHits = new(0);

    public NetworkVariable<bool> ShieldActivate = new(false);
    


    public void TakeDamagePlayer(int enemyDamage)
    {
        

        if(!IsServer) return;

        if (ShieldActivate.Value)
        {
            ShieldHits.Value--;
            Debug.Log(ShieldHits.Value);

            if (ShieldHits.Value <= 0)
            {
                ShieldHits.Value = 0;
                ShieldActivate.Value = false;

            }
            
            return;

        }

        Healt.Value -= enemyDamage;

        if(Healt.Value <= 0)
        {
            NetworkObject.Despawn();
        
        }


    }
}
