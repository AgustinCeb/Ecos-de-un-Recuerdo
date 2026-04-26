using UnityEngine;
[CreateAssetMenu(fileName ="ItemData",menuName ="Inventory/NewItem")]



public class ItemData : ScriptableObject
{
    public enum ItemType
    {
        Consumable,
        Equipment,
        Material,

    }
    //Base Info
    [SerializeField] private string _itemName;
    [SerializeField] private Sprite _itemIcon;
    [SerializeField] private string _itemDescription;
    //Item ID
    [SerializeField] private int    _itemID;
    //Item Type
    [SerializeField] private ItemType _itemType;
    //Item Stack
    [SerializeField] private bool _isStackable;
    [SerializeField] private int _maxStack = 1;

    public string ItemName => _itemName;
    public Sprite ItemIcon => _itemIcon;
    public string ItemDescription => _itemDescription;
    public int ItemID => _itemID;
    public ItemType Type => _itemType;
    public bool IsStackable => _isStackable;
    public int MaxStack => _maxStack;
    

}
