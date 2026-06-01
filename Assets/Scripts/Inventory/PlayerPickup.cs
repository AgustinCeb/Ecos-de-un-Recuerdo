using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;

public class PlayerPickup : NetworkBehaviour
{
    [SerializeField] private Inventory _inventory;

    private void OnTriggerEnter(Collider other)
    {
        ItemPickup pickup = other.GetComponent<ItemPickup>();

        if(pickup != null)
        {
            _inventory.addItem(pickup.ItemData, pickup.Quantity);

            
            if (IsServer)
            {
                NetworkObject.Despawn();
            }
        }
    }
}
