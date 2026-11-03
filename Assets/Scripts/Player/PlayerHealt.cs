using UnityEngine;
using Unity.Netcode;
using UnityEditorInternal;

public class PlayerHealt : NetworkBehaviour
{
    int maxHealth;

    public NetworkVariable <int> Healt = new(100);

    public NetworkVariable<int> ShieldHits = new(0);

    public NetworkVariable<bool> ShieldActivate = new(false);

    public override void OnNetworkSpawn()
    {
        if (!IsServer)return;

        int startingMaxHp = maxHealth;
        maxHealth = GetComponent<PlayerStats>().getHealthPoints();
        
    }

    private void Update()
    {
        if (!IsServer) return;

        int newMaxHealth = GetComponent<PlayerStats>().getHealthPoints();

        if (newMaxHealth != maxHealth)
        {
            maxHealth = newMaxHealth;

            // Solo reducir la vida si supera el nuevo máximo.
            if (Healt.Value > maxHealth)
            {
                Healt.Value = maxHealth;
            }
        }
    }

    public void RestoreHealth()
    {
        if (!IsServer) return;

        Healt.Value = maxHealth;
    }

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

    public bool HealPlayer(int amount)
    {
        if (!IsServer) return false;


        if (Healt.Value >= maxHealth)
            return false;

        Healt.Value = Mathf.Min(Healt.Value + amount, maxHealth);

        return true;
    }

}
