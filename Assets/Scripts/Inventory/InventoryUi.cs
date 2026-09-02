
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryUi : MonoBehaviour
{
    [Header("Inventarios")]
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Inventory2 _inventory2;
    [SerializeField] private Inventory3 _inventory3;
    [Header("Slot")]
    [SerializeField] private GameObject _slotPrefab;
    [Header ("Contenedor Inventario")]
    [SerializeField] private Transform _content1;
    [SerializeField] private Transform _content2;
    [SerializeField] private Transform _content3;
    [Header("Slots Maximos")]
    [SerializeField] private int _maxSlots = 18;

    private bool _inventoryFound;

    private List<InventorySlotUi> _uiSlots1 = new();
    private List<InventorySlotUi> _uiSlots2 = new();
    private List<InventorySlotUi> _uiSlots3 = new();

    private void Update()
    {
        if(_inventoryFound) return;
        {
            Inventory[] inventories = Object.FindObjectsByType<Inventory>();
            Inventory2[] inventory2s = Object.FindObjectsByType<Inventory2>();
            Inventory3[] inventory3s = Object.FindObjectsByType<Inventory3>();


            foreach (Inventory inventory in inventories)
            {
                if (inventory.IsOwner)
                {
                    _inventory = inventory;
                    
                    break;
                }
                

            }

            foreach (Inventory2 inventory2 in inventory2s)
            {
                if (inventory2.IsOwner)
                {
                    _inventory2 = inventory2;
                    
                    break;

                }

            }

            foreach (Inventory3 inventory3 in inventory3s)
            {
                if (inventory3.IsOwner)
                {
                    _inventory3 = inventory3;
                    

                    break;

                }


            }

            if (_inventory != null && _inventory2 != null && _inventory3 != null)
            {
                _inventoryFound = true;

                CreateSlot(_content1,_uiSlots1);
                CreateSlot(_content2, _uiSlots2);
                CreateSlot(_content3, _uiSlots3);

                UpdateUI();

            }

        }

    }

    private void CreateSlot(Transform content,List<InventorySlotUi> uiSolts)
    {
        for (int i =0; i < _maxSlots; i++)
        {
            GameObject slot = Instantiate(_slotPrefab, content);

            uiSolts.Add(slot.GetComponent<InventorySlotUi>());
        }

    }

    public void UpdateUI()
    {
        if (_inventory == null || _inventory2 == null || _inventory3 == null) return;


        UpdateSlots(_inventory.Slots, _uiSlots1);
        UpdateSlots(_inventory2.Slots, _uiSlots2);
        UpdateSlots(_inventory3.Slots, _uiSlots3);

    }

    private void UpdateSlots(List<InventorySlot> inventorySlots,List<InventorySlotUi> uiSlots)
    {
        for(int i = 0; i < uiSlots.Count; i++)
        {
            if (i< inventorySlots.Count)
            {
                uiSlots[i].SetSlot(inventorySlots[i]);
            }
            else
            {
                uiSlots[i].SetSlot(null);
            }

        }

    }

}
