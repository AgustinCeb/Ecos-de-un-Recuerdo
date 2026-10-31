using UnityEngine;
using static UnityEditor.Progress;
[CreateAssetMenu(fileName ="ItemData",menuName ="Inventory/NewItem")]



public class ItemData : ScriptableObject
{
    public enum ItemType
    {
        Consumable,
        Equipment,
        Material,

    }

    public enum RuneSlotType
    {
        None,
        Feet,
        Head,
        Body
    }

    public enum RuneEffectType
    {
        None,
        MaxHealth,
        MaxMana,
        Damage,
        Defense,
        Agility
    }

    public enum RuneEffectMode
    {
        Flat,
        Percentage
    }

    [Header("Base Info")]
    [SerializeField] private string _itemName;
    [SerializeField] private Sprite _itemIcon;
    [SerializeField] private string _itemDescription;
    
    [Header("Item ID")]
    [SerializeField] private int    _itemID;
    
    [Header("Item Type")]
    [SerializeField] private ItemType _itemType;
    
    [Header("Item Stack")]
    [SerializeField] private bool _isStackable;
    [SerializeField] private int _maxStack = 1;

    [Header("Rune Settings")]
    [SerializeField] private RuneSlotType _runeSlot;
    [SerializeField] private RuneEffectType _runeEffect;
    [SerializeField] private float _effectValue;
    [SerializeField] private RuneEffectMode _effectMode;


    public string ItemName => _itemName;
    public Sprite ItemIcon => _itemIcon;
    public string ItemDescription => _itemDescription;
    public int ItemID => _itemID;
    public ItemType Type => _itemType;
    public bool IsStackable => _isStackable;
    public int MaxStack => _maxStack;

    public RuneSlotType RuneSlot => _runeSlot;
    public RuneEffectType RuneEffect => _runeEffect;
    public float EffectValue => _effectValue;
    public RuneEffectMode EffectMode => _effectMode;


}
