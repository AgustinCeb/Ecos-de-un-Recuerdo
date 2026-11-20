using System.Collections.Generic;
using UnityEngine;

public class InventoryUi : MonoBehaviour
{
    [Header("Inventarios")]
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Inventory2 _inventory2;
    [SerializeField] private Inventory3 _inventory3;

    [Header("Slot")]
    [SerializeField] private GameObject _slotPrefab;

    [Header("Contenedor Inventario")]
    [SerializeField] private Transform _content1;
    [SerializeField] private Transform _content2;
    [SerializeField] private Transform _content3;

    [Header("Slots Maximos")]
    [SerializeField] private int _maxSlots = 18;

    [Header("Panel de información")]
    [SerializeField] private ItemInfoUI _itemInfoUI;

    [Header("Equipamiento")]
    [SerializeField] private RuneEquipUI _runeEquipUI;

    private RuenEquip _runeEquipment;

    private List<InventorySlotUi> _uiSlots1 = new();
    private List<InventorySlotUi> _uiSlots2 = new();
    private List<InventorySlotUi> _uiSlots3 = new();

    private bool _slotsCreated;

    private void OnEnable()
    {
        FindInventories();
        RefreshEquipUI();
    }

    public void FindInventories()
    {
        Inventory[] inventories =Object.FindObjectsByType<Inventory>(FindObjectsSortMode.None);

        Inventory newInventory = null;

        // Encontrar el inventario del personaje local.
        foreach (Inventory inventory in inventories)
        {
            if (inventory.IsOwner && inventory.IsSpawned)
            {
                newInventory = inventory;
                break;
            }
        }


        if (newInventory == null)
        {
            Debug.LogWarning("Todavía no se encontró el inventario del jugador propietario.");
            return;
        }

        // Obtener los otros inventarios del mismo personaje.
        Inventory2 newInventory2 = newInventory.GetComponent<Inventory2>();
        Inventory3 newInventory3 = newInventory.GetComponent<Inventory3>();
        RuenEquip newRuneEquipment = newInventory.GetComponent<RuenEquip>();

        if (newInventory2 == null || newInventory3 == null)
        {
            Debug.LogError("Falta alguno de los inventarios en el personaje actual.");
            return;
        }

        if (newRuneEquipment == null)
        {
            Debug.LogError("No se encontró RuenEquip en el personaje actual.");
            return;
        }

        // Actualizar las referencias.
        _inventory = newInventory;
        _inventory2 = newInventory2;
        _inventory3 = newInventory3;
        _runeEquipment = newRuneEquipment;

        if (_runeEquipUI != null)
            _runeEquipUI.SetRuneEquipment(_runeEquipment);

        // Crear los slots solamente la primera vez.
        if (!_slotsCreated)
        {
            CreateSlot(_content1, _uiSlots1);
            CreateSlot(_content2, _uiSlots2);
            CreateSlot(_content3, _uiSlots3);

            _slotsCreated = true;
        }

        // Actualizar las referencias de los slots existentes.
        UpdateSlotReferences(_uiSlots1);
        UpdateSlotReferences(_uiSlots2);
        UpdateSlotReferences(_uiSlots3);

        
        UpdateUI();
        RefreshEquipUI();
    }

    private void CreateSlot(
        Transform content,
        List<InventorySlotUi> uiSlots)
    {
        for (int i = 0; i < _maxSlots; i++)
        {
            GameObject slot = Instantiate(_slotPrefab, content);

            InventorySlotUi slotUI =
                slot.GetComponent<InventorySlotUi>();

            slotUI.SetInfoPanel(_itemInfoUI);
            slotUI.SetContextReferences(_runeEquipment, this, _inventory3);

            uiSlots.Add(slotUI);
        }
    }

    private void UpdateSlotReferences(List<InventorySlotUi> uiSlots)
    {
        foreach (InventorySlotUi slot in uiSlots)
        {
            slot.SetContextReferences(_runeEquipment, this, _inventory3);
        }
    }

    public void UpdateUI()
    {
        // Comprobar que seguimos vinculados al personaje actual.
        if (_inventory == null ||
            _inventory2 == null ||
            _inventory3 == null)
        {
            FindInventories();
            return;
        }

        UpdateSlots(_inventory.Slots, _uiSlots1);
        UpdateSlots(_inventory2.Slots, _uiSlots2);
        UpdateSlots(_inventory3.Slots, _uiSlots3);
    }

    public void RefreshEquipUI()
    {
        if (_runeEquipUI != null)
            _runeEquipUI.RefreshUI();
    }

    private void UpdateSlots(List<InventorySlot> inventorySlots,List<InventorySlotUi> uiSlots)
    {
        for (int i = 0; i < uiSlots.Count; i++)
        {
            if (i < inventorySlots.Count)
                uiSlots[i].SetSlot(inventorySlots[i]);
            else
                uiSlots[i].SetSlot(null);
        }
    }
}