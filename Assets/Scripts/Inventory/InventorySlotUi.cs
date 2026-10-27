using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUi : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _quantityText;

    private InventorySlot _currentSlot;

    public void SetSlot(InventorySlot slot) 
    {
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_currentSlot == null)
            return;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Clic derecho sobre: " + _currentSlot.ItemData.ItemName);

            // En el siguiente paso abriremos el menú contextual.
        }
    }

}

