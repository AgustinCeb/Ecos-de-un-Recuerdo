using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Inventory : NetworkBehaviour
{
    [Header("Rune Inventory Max Slots")]
    [SerializeField] private int _maxSlot = 18;

    private List<InventorySlot> _slots = new List<InventorySlot>();


    public List<InventorySlot> Slots => _slots;
    public bool HasFreeSlot => _slots.Count < _maxSlot;


    public void SaveInventory(PlayerData data)
    {
        if (data == null) return;

        if (!IsOwner) return;

        data.Inventory.Clear();

        foreach (InventorySlot slot in _slots)
        {
            if (slot.ItemData == null || slot.Quantity <= 0)
                continue;

            data.Inventory.Add(new SavedItem(
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

        data.Inventory.Clear();

        for (int i = 0; i < itemIDs.Length; i++)
        {
            if (quantities[i] <= 0) continue;

            data.Inventory.Add(new SavedItem(itemIDs[i], quantities[i]));
        }

        Debug.Log($"[SYNC INVENTORY 1 ] Jugador: {OwnerClientId} | Slots: {data.Inventory.Count}");
    }
    //

    public void RestoreInventory(PlayerData data, ItemDictionary dictionary)
    {
        if (data == null || dictionary == null) return;

        _slots.Clear();

        foreach (SavedItem savedItem in data.Inventory)
        {
            ItemData item = dictionary.GetItem(savedItem.ItemID);

            if (item == null || savedItem.Quantity <= 0)
                continue;

            if (_slots.Count >= _maxSlot)
            {
                Debug.LogWarning("No hay espacio suficiente para restaurar el inventario.");
                break;
            }

            _slots.Add(new InventorySlot(item, savedItem.Quantity));
        }
    }
    //
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


        Debug.Log($"[CLIENT RESTORE INVENTORY1] Slots: {_slots.Count}");
    }

    //
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
            _slots.Add(new InventorySlot(itemData,amount));
            SyncInventoryToServer();
            GameManager.Instance.ShowNotification($"Se obtuvo {itemData.ItemName} X {amount}");
            return true;

        }
        else
        {
            Debug.Log ("Inventario lleno");
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
    public bool HasItem(ItemData itemData)
    {
        foreach (var slot in _slots)
        {
            if (slot.ItemData == itemData && slot.Quantity > 0)
                return true;
        }

        return false;
    }

    public bool RemoveItem(ItemData itemData, int amount = 1)
    {
        if (itemData == null || amount <= 0)
            return false;

        foreach (var slot in _slots)
        {
            if (slot.ItemData != itemData || slot.Quantity < amount)
                continue;

            slot.Quantity -= amount;
            if (slot.Quantity <= 0)
                _slots.Remove(slot);

            SyncInventoryToServer();

            return true;
        }

        return false;
    }

}
