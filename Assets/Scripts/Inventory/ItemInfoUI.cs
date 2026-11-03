using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoUI : MonoBehaviour
{
    [Header("Información del objeto")]
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TMP_Text _itemName;
    [SerializeField] private TMP_Text _itemDescription;

    public void Show(ItemData itemData)
    {
        if (itemData == null)
            return;

        _itemIcon.sprite = itemData.ItemIcon;
        _itemIcon.enabled = itemData.ItemIcon != null;

        _itemName.text = itemData.ItemName;
        _itemDescription.text = itemData.ItemDescription;

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
