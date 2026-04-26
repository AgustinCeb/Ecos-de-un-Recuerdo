using Unity.VisualScripting;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;

    private void OnTriggerEnter(Collider other)
    {
        ItemPickup pickup = other.GetComponent<ItemPickup>();

        if(pickup != null)
        {
            _inventory.addItem(pickup.ItemData, pickup.Quantity);
            Debug.Log("Item Guardado");
            Destroy(other.gameObject);
        }
    }
}
