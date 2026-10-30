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

    public void Show(ItemData itemData, Vector2 position, ItemInfoUI infoUI)
    {
        _currentItem = itemData;
        _itemInfoUI = infoUI;

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

        _infoButton.onClick.RemoveAllListeners();
        _infoButton.onClick.AddListener(ShowInfo);

        _exitButton.onClick.RemoveAllListeners();
        _exitButton.onClick.AddListener(Hide);

        gameObject.SetActive(true);
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
