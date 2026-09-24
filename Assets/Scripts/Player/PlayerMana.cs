using Unity.Netcode;
using UnityEngine;

public class PlayerMana : NetworkBehaviour
{
    int maxMana;

    public NetworkVariable<int> Mana = new(100);

    

    private void Update()
    {
        maxMana = GetComponent<PlayerStats>().getMaxMana();

        if(Mana.Value > maxMana)
            Mana.Value = maxMana;

    }

    public bool TryUseMana(int amount)
    {
        if (Mana.Value < amount)
            return false;

        Mana.Value -= amount;
        return true;

    }

}
