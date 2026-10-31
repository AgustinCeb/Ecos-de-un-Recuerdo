using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUi : MonoBehaviour, IPointerClickHandler
{
    [Header("Datos Principales")]
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _quantityText;

    [Header("Menu De Acciones")]
    [SerializeField] private InventoryContentMenu _contextMenuPrefab;

    [Header("Panel de informacion")]
    [SerializeField] private ItemInfoUI _itemInfoUI;

    private RuenEquip _runeEquipment;
    private InventoryUi _inventoryUi;

    private InventorySlot _currentSlot;

    private static InventoryContentMenu _contextMenu;

    public void SetSlot(InventorySlot slot) 
    {
        _currentSlot = slot;

        if (slot == null)
        {
            _icon.enabled = false;
            _itemNameText.text = "";
            _quantityText.text = "";
            return;
        }

        _icon.enabled = true;
        _icon.sprite = slot.ItemData.ItemIcon;
        _itemNameText.text = slot.ItemData.ItemName;
        _quantityText.text = slot.Quantity.ToString();

    }

    public void SetInfoPanel(ItemInfoUI infoUI)
    {
        _itemInfoUI = infoUI;
    }

    public void SetContextReferences(RuenEquip runeEquipment, InventoryUi inventoryUi)
    {
        _runeEquipment = runeEquipment;
        _inventoryUi = inventoryUi;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_currentSlot == null)
            return;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (_contextMenu == null)
            {
                Canvas canvas = GetComponentInParent<Canvas>();

                if (canvas == null)
                {
                    Debug.LogError("No se encontró el Canvas del inventario.");
                    return;
                }

                _contextMenu = Instantiate(_contextMenuPrefab, canvas.transform);

                _contextMenu.Hide();
            }

            _contextMenu.Show(_currentSlot.ItemData, eventData.position, _itemInfoUI,_runeEquipment, _inventoryUi);
        }
    }

}

