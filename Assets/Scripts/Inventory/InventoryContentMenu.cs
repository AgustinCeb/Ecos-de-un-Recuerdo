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

    public void Show(ItemData itemData, Vector2 position, ItemInfoUI infoUI, RuenEquip runeEquipment, InventoryUi inventoryUi)
    {
        _currentItem = itemData;
        _itemInfoUI = infoUI;

        _runeEquipment = runeEquipment;
        _inventoryUi = inventoryUi;

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

        _actionButton.onClick.RemoveAllListeners();
        _actionButton.onClick.AddListener(ExecuteAction);

        _infoButton.onClick.RemoveAllListeners();
        _infoButton.onClick.AddListener(ShowInfo);

        _exitButton.onClick.RemoveAllListeners();
        _exitButton.onClick.AddListener(Hide);

        gameObject.SetActive(true);
    }

    private void ExecuteAction()
    {
        if (_currentItem == null)
            return;

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
                Hide();
            }
        }
    }

    private void ShowInfo()
    {
        Hide();
        _itemInfoUI.Show(_currentItem);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
