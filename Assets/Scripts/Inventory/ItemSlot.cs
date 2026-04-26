using System;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public ItemData ItemData;
    public int Quantity;

    public InventorySlot(ItemData itemData, int quantity)
    {
        ItemData = itemData;
        Quantity = quantity;
    }
}
