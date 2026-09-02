using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Inventory2 : NetworkBehaviour
{
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
            _slots.Add(new InventorySlot(itemData, amount));

        }
        else
        {
            Debug.Log("Inventario lleno");
        }

    }
}
