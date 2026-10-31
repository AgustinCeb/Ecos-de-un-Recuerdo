using UnityEngine;
using UnityEngine.UI;

public class RuneEquipUI : MonoBehaviour
{
    [Header("Referencia al equipamiento")]
    [SerializeField] private RuenEquip _runeEquipment;

    [Header("Iconos de las ranuras")]
    [SerializeField] private Image _headIcon;
    [SerializeField] private Image _bodyIcon;
    [SerializeField] private Image _feetIcon;

    [Header("Interacción de las ranuras")]
    [SerializeField] private EquipSlotUI _headSlot;
    [SerializeField] private EquipSlotUI _bodySlot;
    [SerializeField] private EquipSlotUI _feetSlot;

    [Header("Información y menú")]
    [SerializeField] private ItemInfoUI _itemInfoUI;
    [SerializeField] private InventoryUi _inventoryUi;

    private void OnEnable()
    {
        RefreshUI();
    }

    public void SetRuneEquipment(RuenEquip runeEquipment)
    {
        _runeEquipment = runeEquipment;

        if (_headSlot != null)
            _headSlot.SetRuneEquipment(_runeEquipment);

        if (_bodySlot != null)
            _bodySlot.SetRuneEquipment(_runeEquipment);

        if (_feetSlot != null)
            _feetSlot.SetRuneEquipment(_runeEquipment);

        RefreshUI();
    }

    public void RefreshUI()
    {
        if (_runeEquipment == null)
            return;

        UpdateIcon(_feetIcon, _runeEquipment.FeetRune);
        UpdateIcon(_headIcon, _runeEquipment.HeadRune);
        UpdateIcon(_bodyIcon, _runeEquipment.BodyRune);
    }

    private void UpdateIcon(Image icon, ItemData rune)
    {
        if (icon == null)
            return;

        if (rune == null)
        {
            icon.sprite = null;
            icon.enabled = false;
            return;
        }

        icon.sprite = rune.ItemIcon;
        icon.enabled = true;
    }

}
