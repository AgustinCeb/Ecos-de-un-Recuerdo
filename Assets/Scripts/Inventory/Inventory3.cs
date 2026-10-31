using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Inventory3 : NetworkBehaviour
{
    [SerializeField] private int _maxSlot = 18;

    private List<InventorySlot> _slots = new List<InventorySlot>();

    public List<InventorySlot> Slots => _slots;

    

    public void addItem(ItemData itemData, int amount)
    {
        if(!IsServer) return;

        if (itemData.IsStackable)
        {
            foreach (var slot in _slots)
            {
                if (slot.ItemData == itemData)
                {
                    slot.Quantity += amount;
                    return;
                }

            }

        }

        if (Slots.Count < _maxSlot)
        {
            _slots.Add(new InventorySlot(itemData, amount));

        }
        else
        {
            Debug.Log("Inventario lleno");
        }

    }

    public bool RemoveItem(ItemData itemData, int amount)
    {
        foreach (var slot in _slots)
        {
            if (slot.ItemData == itemData)
            {
                if (slot.Quantity < amount)
                    return false;

                slot.Quantity -= amount;

                if (slot.Quantity <= 0)
                    _slots.Remove(slot);

                return true;
            }
        }

        return false;
    }

    public int GetItemCount(ItemData itemData)
    {
        foreach (var slot in _slots)
        {
            if (slot.ItemData == itemData)
                return slot.Quantity;
        }

        return 0;
    }

    [ServerRpc]
    public void UsePotionServerRpc(int itemID)
    {
        ItemData potion = null;

        foreach (var slot in _slots)
        {
            if (slot.ItemData.ItemID == itemID)
            {
                potion = slot.ItemData;
                break;
            }
        }

        if (potion == null)
            return;

        if (potion.Type != ItemData.ItemType.Consumable)
            return;

        bool effectApplied = false;

        switch (potion.ConsumableEffect)
        {
            case ItemData.ConsumableEffectType.Health:

                PlayerHealt health = GetComponent<PlayerHealt>();

                if (health != null)
                    effectApplied = health.HealPlayer(potion.ConsumableEffectValue);

                break;

            case ItemData.ConsumableEffectType.Mana:

                PlayerMana mana = GetComponent<PlayerMana>();

                if (mana != null)
                    effectApplied = mana.RestoreMana(potion.ConsumableEffectValue);

                break;
        }

        if (!effectApplied)
            return;

        RemoveItem(potion, 1);
    }

}
