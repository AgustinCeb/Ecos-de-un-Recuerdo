using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class Inventory2 : NetworkBehaviour
{
    [Header("Max Slots Materiales")]
    [SerializeField] private int _maxSlot = 18;

    private List<InventorySlot> _slots = new List<InventorySlot>();

    public List<InventorySlot> Slots => _slots;

    Dictionary<int, int> itemsCountCache = new();
    public event Action OnInventoryChanged;

    public void SaveInventory(PlayerData data)
    {
        if (data == null) return;

        // Si es un jugador cliente, los datos ya fueron sincronizados por RPC.
        if (!IsOwner) return;

        data.Inventory2.Clear();

        foreach (InventorySlot slot in _slots)
        {
            if (slot.ItemData == null || slot.Quantity <= 0)
                continue;

            data.Inventory2.Add(new SavedItem(
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

        data.Inventory2.Clear();

        for (int i = 0; i < itemIDs.Length; i++)
        {
            if (quantities[i] <= 0) continue;

            data.Inventory2.Add(new SavedItem(itemIDs[i], quantities[i]));
        }

        Debug.Log($"[SYNC INVENTORY2] Jugador: {OwnerClientId} | Slots: {data.Inventory2.Count}");
    }
    //
    public void RestoreInventory(PlayerData data, ItemDictionary dictionary)
    {
        if (data == null || dictionary == null) return;

        _slots.Clear();

        foreach (SavedItem savedItem in data.Inventory2)
        {
            ItemData item = dictionary.GetItem(savedItem.ItemID);

            if (item == null || savedItem.Quantity <= 0)
                continue;

            if (_slots.Count >= _maxSlot)
            {
                Debug.LogWarning("No hay espacio suficiente para restaurar el inventario de materiales.");
                break;
            }

            _slots.Add(new InventorySlot(item, savedItem.Quantity));
        }

        itemsCountCache.Clear();

        OnInventoryChanged?.Invoke();


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

        itemsCountCache.Clear();
        OnInventoryChanged?.Invoke();

        Debug.Log($"[CLIENT RESTORE INVENTORY2] Slots: {_slots.Count}");
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
                    OnInventoryChanged?.Invoke();
                    SyncInventoryToServer();
                    GameManager.Instance.ShowNotification($"Se obtuvo {itemData.ItemName} X {amount}");
                    return true;
                }

            }

        }

        if (Slots.Count < _maxSlot)
        {
            _slots.Add(new InventorySlot(itemData, amount));
            OnInventoryChanged?.Invoke();
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

    public int GetItemCount(ItemData itemData)
    {
        int total = 0;

        foreach(var slot in _slots)
        {
            if (slot.ItemData == itemData) 
            {
                total += slot.Quantity;
            }

        }

        return total;
    }

    public bool RemoveItem(ItemData itemData, int amount)
    {
        if (itemData == null || amount <= 0)
        {
            return false;
        }

        // Comprobar que haya suficientes unidades.
        if (GetItemCount(itemData) < amount)
        {
            return false;
        }

        int remaining = amount;

        // Recorrer desde el final para poder eliminar slots.
        for (int i = _slots.Count - 1; i >= 0; i--)
        {
            if (_slots[i].ItemData != itemData)
            {
                continue;
            }

            int removeAmount = Mathf.Min(
                _slots[i].Quantity,
                remaining
            );

            _slots[i].Quantity -= removeAmount;
            remaining -= removeAmount;
            SyncInventoryToServer();

            // Eliminar el slot si quedó vacío.
            if (_slots[i].Quantity <= 0)
            {
                _slots.RemoveAt(i);
            }

            if (remaining <= 0)
            {
                break;
            }
        }

        // Avisar a la UI y al sistema de misiones.
        OnInventoryChanged?.Invoke();

        return true;
    }

}
