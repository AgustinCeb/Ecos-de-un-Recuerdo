
using Unity.VisualScripting;
using UnityEngine;

public class InventoryUi : MonoBehaviour
{

    [SerializeField] private Inventory _inventory;
    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private Transform _contentParent;

    private bool _inventoryFound;

    private void Update()
    {
        if(_inventoryFound) return;
        {
            Inventory[] inventories = Object.FindObjectsByType<Inventory>(FindObjectsSortMode.None);


            foreach (Inventory inventory in inventories)
            {
                if (inventory.IsOwner)
                {
                    _inventory = inventory;
                    _inventoryFound = true;
                    break;
                }
            }
     
        }

    }

    public void UpdateUI()
    {
        if(_inventory == null) return;

        foreach (Transform child in _contentParent )
        {
            Destroy( child.gameObject );
        }

        foreach (var slot in _inventory.Slots)
        {
            GameObject newSlot = Instantiate(_slotPrefab, _contentParent);
            InventorySlotUi slotUi = newSlot.GetComponent<InventorySlotUi>();
            
            slotUi.SetSlot( slot );

        }
    }

}
