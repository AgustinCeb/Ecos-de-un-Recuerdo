using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Inventory3 : NetworkBehaviour
{
    [Header("Max Slots Pociones")]
    [SerializeField] private int _maxSlot = 18;

    private List<InventorySlot> _slots = new List<InventorySlot>();

    public List<InventorySlot> Slots => _slots;


    public void SaveInventory(PlayerData data)
    {
        if (!IsServer || data == null) return;

        if (!IsOwner) return;

        data.Inventory3.Clear();

        foreach (InventorySlot slot in _slots)
        {
            if (slot.ItemData == null || slot.Quantity <= 0)
                continue;

            data.Inventory3.Add(new SavedItem(
                slot.ItemData.ItemID,
                slot.Quantity
            ));

            
        }

    }
    //
    private void SyncInventoryToServer()
    {
        if (!IsOwner || !IsSpawned) return;

        List<int> itemIDs = new();
        List<int> quantities = new();

        foreach (InventorySlot slot in _slots)
        {
            if (slot.ItemData == null || slot.Quantity <= 0)
                continue;

            itemIDs.Add(slot.ItemData.ItemID);
            quantities.Add(slot.Quantity);
        }

        UpdateServerInventoryRpc(itemIDs.ToArray(), quantities.ToArray());
    }

    [Rpc(SendTo.Server)]
    private void UpdateServerInventoryRpc(int[] itemIDs, int[] quantities)
    {
        if (!IsServer) return;

        if (itemIDs == null || quantities == null) return;
        if (itemIDs.Length != quantities.Length) return;

        PlayerDataManager dataManager = PlayerDataManager.Instance;
        if (dataManager == null) return;

        PlayerData data = dataManager.GetOrCreateData(OwnerClientId);
        if (data == null) return;

        data.Inventory3.Clear();

        for (int i = 0; i < itemIDs.Length; i++)
        {
            if (quantities[i] <= 0) continue;

            data.Inventory3.Add(new SavedItem(itemIDs[i], quantities[i]));
        }

        Debug.Log($"[SYNC INVENTORY3] Jugador: {OwnerClientId} | Slots: {data.Inventory2.Count}");
    }
    //

    public void RestoreInventory(PlayerData data, ItemDictionary dictionary)
    {

        if (data == null || dictionary == null) return;

        _slots.Clear();

        

        foreach (SavedItem savedItem in data.Inventory3)
        {
            ItemData item = dictionary.GetItem(savedItem.ItemID);

            

            if (item == null || savedItem.Quantity <= 0)
                continue;

            if (_slots.Count >= _maxSlot)
            {
                Debug.LogWarning("No hay espacio suficiente para restaurar el inventario de pociones.");
                break;
            }

            _slots.Add(new InventorySlot(item, savedItem.Quantity));

            
        }

    }


    public void SendRestoredInventoryToOwner()
    {

        Debug.Log($"[SEND RESTORE] Server: {IsServer} | Spawned: {IsSpawned} | Owner: {OwnerClientId} | Slots: {_slots.Count}");

        if (!IsServer || !IsSpawned) return;

        List<int> itemIDs = new();
        List<int> quantities = new();

        foreach (InventorySlot slot in _slots)
        {
            if (slot.ItemData == null || slot.Quantity <= 0)
                continue;

            itemIDs.Add(slot.ItemData.ItemID);
            quantities.Add(slot.Quantity);
        }

        RestoreInventoryClientRpc(itemIDs.ToArray(), quantities.ToArray());
    }

    [Rpc(SendTo.Owner)]
    private void RestoreInventoryClientRpc(int[] itemIDs, int[] quantities)
    {

        Debug.Log($"[RECEIVE RESTORE] RPC recibido. Owner: {OwnerClientId} | Es dueño: {IsOwner}");

        if (itemIDs == null || quantities == null) return;
        if (itemIDs.Length != quantities.Length) return;

        ItemDictionary dictionary = FindFirstObjectByType<ItemDictionary>();
        if (dictionary == null) return;

        _slots.Clear();

        for (int i = 0; i < itemIDs.Length; i++)
        {
            ItemData item = dictionary.GetItem(itemIDs[i]);

            if (item == null || quantities[i] <= 0)
                continue;

            if (_slots.Count >= _maxSlot) break;

            _slots.Add(new InventorySlot(item, quantities[i]));
        }

        Debug.Log($"[CLIENT RESTORE INVENTORY3] Slots: {_slots.Count}");
    }


    public bool addItem(ItemData itemData, int amount)
    {
        if (itemData == null || amount <= 0)
            return false;

        if (itemData.IsStackable)
        {
            foreach (var slot in _slots)
            {
                if (slot.ItemData == itemData)
                {
                    slot.Quantity += amount;
                    SyncInventoryToServer();
                    GameManager.Instance.ShowNotification($"Se obtuvo {itemData.ItemName} X {amount}");
                    return true;
                }

            }

        }

        if (Slots.Count < _maxSlot)
        {
            _slots.Add(new InventorySlot(itemData, amount));
            SyncInventoryToServer();
            GameManager.Instance.ShowNotification($"Se obtuvo {itemData.ItemName} X {amount}");
            return true;

        }
        else
        {
            Debug.Log("Inventario lleno");
            GameManager.Instance.ShowNotification("Inventario lleno");
            return false;
        }

    }
    //
    [Rpc(SendTo.Owner)]
    public void ReceiveChestRewardRpc(int itemID, int amount)
    {
        ItemDictionary dictionary = FindFirstObjectByType<ItemDictionary>();

        if (dictionary == null) return;

        ItemData item = dictionary.GetItem(itemID);

        if (item == null) return;

        addItem(item, amount);
    }
    //
    public bool RemoveItem(ItemData itemData, int amount)
    {
        foreach (var slot in _slots)
        {
            if (slot.ItemData == itemData)
            {
                if (slot.Quantity < amount)
                    return false;

                slot.Quantity -= amount;

                if (slot.Quantity <= 0)
                    _slots.Remove(slot);

                SyncInventoryToServer();

                return true;
            }
        }

        return false;
    }

    public int GetItemCount(ItemData itemData)
    {
        foreach (var slot in _slots)
        {
            if (slot.ItemData == itemData)
                return slot.Quantity;
        }

        return 0;
    }

    [ServerRpc]
    public void UsePotionServerRpc(int itemID)
    {
        ItemData potion = null;

        foreach (var slot in _slots)
        {
            if (slot.ItemData.ItemID == itemID)
            {
                potion = slot.ItemData;
                break;
            }
        }

        if (potion == null)
            return;

        if (potion.Type != ItemData.ItemType.Consumable)
            return;

        bool effectApplied = false;

        switch (potion.ConsumableEffect)
        {
            case ItemData.ConsumableEffectType.Health:

                PlayerHealt health = GetComponent<PlayerHealt>();

                if (health != null)
                    effectApplied = health.HealPlayer(potion.ConsumableEffectValue);

                break;

            case ItemData.ConsumableEffectType.Mana:

                PlayerMana mana = GetComponent<PlayerMana>();

                if (mana != null)
                    effectApplied = mana.RestoreMana(potion.ConsumableEffectValue);

                break;
        }

        if (!effectApplied)
            return;

        RemoveItem(potion, 1);
    }

}
