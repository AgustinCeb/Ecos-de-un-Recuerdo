using UnityEngine;

public class RuenEquip : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Inventory _inventory;
    [SerializeField] private PlayerStats _playerStats;

    private ItemData _feetRune;
    private ItemData _headRune;
    private ItemData _bodyRune;

    public ItemData FeetRune => _feetRune;
    public ItemData HeadRune => _headRune;
    public ItemData BodyRune => _bodyRune;

    private void Awake()
    {
        if (_inventory == null)
            _inventory = GetComponent<Inventory>();

        if (_playerStats == null)
            _playerStats = GetComponent<PlayerStats>();
    }

    public bool EquipRune(ItemData rune)
    {
        if (rune == null || rune.Type != ItemData.ItemType.Equipment)
            return false;

        if (!_inventory.HasItem(rune))
            return false;

        ItemData previousRune = GetRuneInSlot(rune.RuneSlot);

        // Comprobar que la ranura sea válida.
        if (rune.RuneSlot == ItemData.RuneSlotType.None)
            return false;

        // Retirar la nueva runa del inventario.
        if (!_inventory.RemoveItem(rune))
            return false;

        // Devolver la runa anterior al inventario.
        if (previousRune != null)
            _inventory.addItem(previousRune, 1);

        // Equipar la nueva runa.
        SetRuneInSlot(rune.RuneSlot, rune);

        RecalculateStats();

        return true;
    }

    private ItemData GetRuneInSlot(ItemData.RuneSlotType slot)
    {
        switch (slot)
        {
            case ItemData.RuneSlotType.Feet:
                return _feetRune;

            case ItemData.RuneSlotType.Head:
                return _headRune;

            case ItemData.RuneSlotType.Body:
                return _bodyRune;

            default:
                return null;
        }
    }

    private void SetRuneInSlot(ItemData.RuneSlotType slot, ItemData rune)
    {
        switch (slot)
        {
            case ItemData.RuneSlotType.Feet:
                _feetRune = rune;
                break;

            case ItemData.RuneSlotType.Head:
                _headRune = rune;
                break;

            case ItemData.RuneSlotType.Body:
                _bodyRune = rune;
                break;
        }
    }

    private void RecalculateStats()
    {
        float hp = 0;
        float mana = 0;
        float damage = 0;
        float defense = 0;
        float agility = 0;

        ApplyRune(_feetRune, ref hp, ref mana, ref damage, ref defense, ref agility);
        ApplyRune(_headRune, ref hp, ref mana, ref damage, ref defense, ref agility);
        ApplyRune(_bodyRune, ref hp, ref mana, ref damage, ref defense, ref agility);

        _playerStats.SetRuneStats(hp, mana, damage, defense, agility);
    }

    private void ApplyRune(ItemData rune, ref float hp, ref float mana, ref float damage, ref float defense, ref float agility)
    {
        if (rune == null)
            return;

        float value = rune.EffectValue;

        switch (rune.EffectMode)
        {
            case ItemData.RuneEffectMode.Flat:
                break;

            case ItemData.RuneEffectMode.Percentage:
                switch (rune.RuneEffect)
                {
                    case ItemData.RuneEffectType.MaxHealth:
                        value = _playerStats.getHealthPoints() * value / 100f;
                        break;

                    case ItemData.RuneEffectType.MaxMana:
                        value = _playerStats.getMaxMana() * value / 100f;
                        break;

                    case ItemData.RuneEffectType.Damage:
                        value = _playerStats.getDamage() * value / 100f;
                        break;

                    case ItemData.RuneEffectType.Defense:
                        value = _playerStats.getDefense() * value / 100f;
                        break;

                    case ItemData.RuneEffectType.Agility:
                        value = _playerStats.getAgility() * value / 100f;
                        break;
                }
                break;
        }

        switch (rune.RuneEffect)
        {
            case ItemData.RuneEffectType.MaxHealth:
                hp += value;
                break;

            case ItemData.RuneEffectType.MaxMana:
                mana += value;
                break;

            case ItemData.RuneEffectType.Damage:
                damage += value;
                break;

            case ItemData.RuneEffectType.Defense:
                defense += value;
                break;

            case ItemData.RuneEffectType.Agility:
                agility += value;
                break;
        }
    }
}
