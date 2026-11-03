using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipSlotUI : MonoBehaviour, IPointerClickHandler
{
    [Header("Tipo de ranura")]
    [SerializeField] private ItemData.RuneSlotType _runeSlot;

    [Header("Referencias")]
    [SerializeField] private ItemInfoUI _itemInfoUI;
    [SerializeField] private InventoryUi _inventoryUi;
    [SerializeField] private InventoryContentMenu _contextMenuPrefab;

    private RuenEquip _runeEquipment;

    private static InventoryContentMenu _contextMenu;

    public void SetRuneEquipment(RuenEquip runeEquipment)
    {
        _runeEquipment = runeEquipment;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right)
            return;

        if (_runeEquipment == null)
            return;

        ItemData rune = GetEquippedRune();

        if (rune == null)
            return;

        if (_contextMenu == null)
        {
            Canvas canvas = GetComponentInParent<Canvas>();

            if (canvas == null)
            {
                Debug.LogError("No se encontró un Canvas.");
                return;
            }

            _contextMenu = Instantiate(_contextMenuPrefab, canvas.transform);
            _contextMenu.Hide();
        }

        _contextMenu.ShowUnequip(rune, _runeSlot, eventData.position, _itemInfoUI, _runeEquipment, _inventoryUi);
    }

    private ItemData GetEquippedRune()
    {
        switch (_runeSlot)
        {
            case ItemData.RuneSlotType.Feet:
                return _runeEquipment.FeetRune;

            case ItemData.RuneSlotType.Head:
                return _runeEquipment.HeadRune;

            case ItemData.RuneSlotType.Body:
                return _runeEquipment.BodyRune;

            default:
                return null;
        }
    }

}
