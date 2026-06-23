using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUi : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _quantityText;

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

}

