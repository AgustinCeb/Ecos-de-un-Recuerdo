using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Inventory : NetworkBehaviour
{
    [Header("Rune Inventory Max Slots")]
    [SerializeField] private int _maxSlot = 18;

    private List<InventorySlot> _slots = new List<InventorySlot>();


    public List<InventorySlot> Slots => _slots;

    

    public void addItem(ItemData itemData, int amount)
    {
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
            _slots.Add(new InventorySlot(itemData,amount));

        }
        else
        {
            Debug.Log ("Inventario lleno");
        }

    }

    public bool HasItem(ItemData itemData)
    {
        foreach (var slot in _slots)
        {
            if (slot.ItemData == itemData && slot.Quantity > 0)
                return true;
        }

        return false;
    }

    public bool RemoveItem(ItemData itemData, int amount = 1)
    {
        if (itemData == null || amount <= 0)
            return false;

        foreach (var slot in _slots)
        {
            if (slot.ItemData != itemData || slot.Quantity < amount)
                continue;

            slot.Quantity -= amount;

            if (slot.Quantity <= 0)
                _slots.Remove(slot);

            return true;
        }

        return false;
    }

}
