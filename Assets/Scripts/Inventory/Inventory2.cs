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

    public bool RemoveItem(ItemData itemData, int amount)
    {
        if (itemData == null || amount <= 0)
        {
            return false;
        }

        // Comprobar que haya suficientes unidades.
        if (GetItemCount(itemData) < amount)
        {
            return false;
        }

        int remaining = amount;

        // Recorrer desde el final para poder eliminar slots.
        for (int i = _slots.Count - 1; i >= 0; i--)
        {
            if (_slots[i].ItemData != itemData)
            {
                continue;
            }

            int removeAmount = Mathf.Min(
                _slots[i].Quantity,
                remaining
            );

            _slots[i].Quantity -= removeAmount;
            remaining -= removeAmount;

            // Eliminar el slot si quedó vacío.
            if (_slots[i].Quantity <= 0)
            {
                _slots.RemoveAt(i);
            }

            if (remaining <= 0)
            {
                break;
            }
        }

        // Avisar a la UI y al sistema de misiones.
        OnInventoryChanged?.Invoke();

        return true;
    }

}
