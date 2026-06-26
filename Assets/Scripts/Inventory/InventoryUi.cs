
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryUi : MonoBehaviour
{

    [SerializeField] private Inventory _inventory;
    [SerializeField] private GameObject _slotPrefab;
    [Header ("Contenedor Inventario")]
    [SerializeField] private Transform _content1;
    [SerializeField] private Transform _content2;
    [SerializeField] private Transform _content3;
    [Header("Slots Maximos")]
    [SerializeField] private int _maxSlots = 18;

    private bool _inventoryFound;

    private List<InventorySlotUi> _uiSlots = new();

    private void Update()
    {
        if(_inventoryFound) return;
        {
            Inventory[] inventories = Object.FindObjectsByType<Inventory>();


            foreach (Inventory inventory in inventories)
            {
                if (inventory.IsOwner)
                {
                    _inventory = inventory;
                    _inventoryFound = true;
                
                    for (int i = 0; i < _maxSlots; i++)
                    {
                    GameObject slot = Instantiate(_slotPrefab, _content1);
                    _uiSlots.Add(slot.GetComponent<InventorySlotUi>());
                    }


                    UpdateUI();
                    break;
                }
                

            }

        }

    }

    public void UpdateUI()
    {
        if (_inventory == null) return;

        for (int i = 0; i < _uiSlots.Count; i++)
        {
            if (i < _inventory.Slots.Count)
            {
                _uiSlots[i].SetSlot(_inventory.Slots[i]);
            }
            else
            {
                _uiSlots[i].SetSlot(null);
            }
        }
    }
}
