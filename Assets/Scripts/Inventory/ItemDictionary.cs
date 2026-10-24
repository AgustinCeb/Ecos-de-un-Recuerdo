using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ItemDictionary : MonoBehaviour
{

    public List<ItemData> items;
    private Dictionary<int, ItemData> itemDictionary;


    private void Awake()
    {
        itemDictionary = new Dictionary<int, ItemData>();

        foreach(ItemData item in items)
        {
            itemDictionary[item.ItemID] = item;
        }
    }

    public ItemData GetItem(int itemID)
    {
        itemDictionary.TryGetValue(itemID, out ItemData item);
        if (item == null) 
        {
            Debug.LogWarning($"No se Encontro el Objeto{itemID}");
        }
        return item;
    }

}
