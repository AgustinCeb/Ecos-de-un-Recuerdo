using Unity.Netcode;
using UnityEngine;

public class PlayerMana : NetworkBehaviour
{
    int maxMana;

    public NetworkVariable<int> Mana = new(50);

    

    private void Update()
    {
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
        Mana.Value = GetComponent<PlayerStats>().getMaxMana();
    }

    public bool TryUseMana(int amount)
    {
        if (Mana.Value < amount)
            return false;

        Mana.Value -= amount;
        return true;

    }

}
