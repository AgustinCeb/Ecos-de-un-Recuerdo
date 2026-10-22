using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;
using System;

public class Inventory2 : NetworkBehaviour
{
    [SerializeField] private int _maxSlot = 18;

    private List<InventorySlot> _slots = new List<InventorySlot>();

    public List<InventorySlot> Slots => _slots;

    Dictionary<int, int> itemsCountCache = new();
    public event Action OnInventoryChanged;

    public void addItem(ItemData itemData, int amount)
    {

        if (itemData.IsStackable)
        {
            foreach (var slot in _slots)
            {
                if (slot.ItemData == itemData)
                {
                    slot.Quantity += amount;
                    OnInventoryChanged?.Invoke();
                    return;
                }

            }

        }

        if (Slots.Count < _maxSlot)
        {
            _slots.Add(new InventorySlot(itemData, amount));
            OnInventoryChanged?.Invoke();

        }
        else
        {
            Debug.Log("Inventario lleno");
        }

    }

    public int GetItemCount(ItemData itemData)
    {
        int total = 0;

        foreach(var slot in _slots)
        {
            if (slot.ItemData == itemData) 
            {
                total += slot.Quantity;
            }

        }

        return total;
    }

}
