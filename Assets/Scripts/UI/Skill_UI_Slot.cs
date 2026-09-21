using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Skill_UI_Slot : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _CooldownText;

    public void SetSlot(InventorySlot slot)
    {
        if (slot == null)
        {
            _icon.enabled = false;
            _CooldownText.text = "";
            return;
        }

        _icon.enabled = true;
        _icon.sprite = slot.ItemData.ItemIcon;
        _CooldownText.text = slot.Quantity.ToString();

    }

}
