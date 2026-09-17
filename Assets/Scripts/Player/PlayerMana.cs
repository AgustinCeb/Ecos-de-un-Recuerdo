using Unity.Netcode;
using UnityEngine;

public class PlayerMana : NetworkBehaviour
{
    int maxMana;

    public NetworkVariable<int> Mana = new(100);


    private void Update()
    {
        int startingMaxMana = maxMana;
        maxMana = GetComponent<PlayerStats>().getMaxMana();
        if (startingMaxMana != maxMana) Mana.Value = maxMana;
    }

    
}
