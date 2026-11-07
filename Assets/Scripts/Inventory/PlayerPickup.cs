using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using static ItemData;

public class PlayerPickup : NetworkBehaviour
{
    [Header("Inventarios")]
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Inventory2 _inventory2;
    [SerializeField] private Inventory3 _inventory3;

    public void GiveItem(ItemData itemData, int amount)
    {
        if (itemData == null || amount <= 0) return;

        switch (itemData.Type)
        {
            case ItemType.Equipment:
                _inventory.addItem(itemData, amount);
                break;

            case ItemType.Material:
                _inventory2.addItem(itemData, amount);
                break;

            case ItemType.Consumable:
                _inventory3.addItem(itemData, amount);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ItemPickup pickup = other.GetComponent<ItemPickup>();


        if (pickup != null)
        {

            switch (pickup.ItemData.Type)
            {
            case ItemType.Equipment:
                _inventory.addItem(pickup.ItemData, pickup.Quantity);
                break;

            case ItemType.Material:
                _inventory2.addItem(pickup.ItemData, pickup.Quantity);
                break;

            case ItemType.Consumable:
                _inventory3.addItem(pickup.ItemData, pickup.Quantity);
                break;
            }
            
            if (IsServer)
            {
                pickup.GetComponent<NetworkObject>().Despawn();

            }
        }
    }
}
