using NUnit.Framework.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryContentMenu : MonoBehaviour
{
    [Header("Botones")]
    [SerializeField] private Button _actionButton;
    [SerializeField] private TMP_Text _actionText;
    [SerializeField] private Button _infoButton;
    [SerializeField] private Button _exitButton;

    private ItemInfoUI _itemInfoUI;
    private ItemData _currentItem;

    private RuenEquip _runeEquipment;
    private InventoryUi _inventoryUi;

    private ItemData.RuneSlotType _runeSlot;
    private bool _isUnequipMenu;

    public void Show(ItemData itemData, Vector2 position, ItemInfoUI infoUI, RuenEquip runeEquipment, InventoryUi inventoryUi)
    {
        _currentItem = itemData;
        _itemInfoUI = infoUI;
        _runeEquipment = runeEquipment;
        _inventoryUi = inventoryUi;
        _isUnequipMenu = false;

        transform.position = position;

        bool hasAction = itemData.Type != ItemData.ItemType.Material;

        _actionButton.gameObject.SetActive(hasAction);

        if (itemData.Type == ItemData.ItemType.Equipment)
        {
            _actionText.text = "Equipar";
        }
        else if (itemData.Type == ItemData.ItemType.Consumable)
        {
            _actionText.text = "Usar";
        }

        ConfigureButtons();
        gameObject.SetActive(true);
    }

    public void ShowUnequip(ItemData itemData, ItemData.RuneSlotType runeSlot, Vector2 position, ItemInfoUI infoUI, RuenEquip runeEquipment, InventoryUi inventoryUi)
    {
        _currentItem = itemData;
        _runeSlot = runeSlot;
        _itemInfoUI = infoUI;
        _runeEquipment = runeEquipment;
        _inventoryUi = inventoryUi;
        _isUnequipMenu = true;

        transform.position = position;

        _actionButton.gameObject.SetActive(true);
        _actionText.text = "Desequipar";

        ConfigureButtons();
        gameObject.SetActive(true);
    }


    private void ConfigureButtons()
    {
        _actionButton.onClick.RemoveAllListeners();
        _actionButton.onClick.AddListener(ExecuteAction);

        _infoButton.onClick.RemoveAllListeners();
        _infoButton.onClick.AddListener(ShowInfo);

        _exitButton.onClick.RemoveAllListeners();
        _exitButton.onClick.AddListener(Hide);
    }

    private void ExecuteAction()
    {
        if (_currentItem == null || _runeEquipment == null)
            return;

        if (_isUnequipMenu)
        {
            bool unequipped = _runeEquipment.UnequipRune(_runeSlot);

            if (unequipped)
            {
                _inventoryUi.UpdateUI();
                _inventoryUi.RefreshEquipUI();
                Hide();
            }

            return;
        }

        if (_currentItem.Type == ItemData.ItemType.Equipment)
        {
            if (_runeEquipment == null)
            {
                Debug.LogError("No se encontró RuneEquipment.");
                return;
            }

            bool equipped = _runeEquipment.EquipRune(_currentItem);

            if (equipped)
            {
                _inventoryUi.UpdateUI();
                _inventoryUi.RefreshEquipUI();
                Hide();
            }
        }
    }

    private void ShowInfo()
    {
        Hide();
        if(_itemInfoUI != null)
        {
            _itemInfoUI.Show(_currentItem);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
