using Unity.VisualScripting;
using UnityEngine;

public class InventoryUi : MonoBehaviour
{

    [SerializeField] private Inventory _inventory;
    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private Transform _contentParent;

    private void Update()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
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
