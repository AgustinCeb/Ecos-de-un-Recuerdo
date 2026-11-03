using Unity.Netcode;
using UnityEngine;

public class PlayerMana : NetworkBehaviour
{
    int maxMana;

    public NetworkVariable<int> Mana = new(50);



    private void Update()
    {
        if (!IsServer) return; 

        int newMaxMana = GetComponent<PlayerStats>().getMaxMana();

        if (newMaxMana != maxMana)
        {
            maxMana = newMaxMana;

            if(Mana.Value > maxMana)
            {
                Mana.Value = maxMana;
            }
        }

        
    }

    public void RestoreMana()
    {
        //Restaura al subir de Nivel
        Mana.Value = GetComponent<PlayerStats>().getMaxMana();
    }

    public bool RestoreMana(int amount)
    {
        //Restaura por Pocion
        if (!IsServer) return false;

        int max = GetComponent<PlayerStats>().getMaxMana();

        if (Mana.Value >= max)
            return false;

        Mana.Value = Mathf.Min(Mana.Value + amount, max);

        return true;
    }

    public bool TryUseMana(int amount)
    {
        if (!IsServer) return false;

        if (Mana.Value < amount)
            return false;

        Mana.Value -= amount;
        return true;

    }

}
