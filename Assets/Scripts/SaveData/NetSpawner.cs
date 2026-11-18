using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetSpawner : NetworkBehaviour
{

    public static NetSpawner Instance { get; private set; }

    [Header("Puntos de aparición del pueblo")]
    [SerializeField] private Transform[] _townSpawnPoints = new Transform[4];

    private Dictionary<ulong, int> _playerSlots = new();

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;

        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            AssignPlayerSlot(clientId);
        }

        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsServer || NetworkManager.Singleton == null) return;

        NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnected;
    }

    private void HandleClientConnected(ulong clientId)
    {
        AssignPlayerSlot(clientId);
    }

    private void HandleClientDisconnected(ulong clientId)
    {
        _playerSlots.Remove(clientId);
    }

    private void AssignPlayerSlot(ulong clientId)
    {
        if (_playerSlots.ContainsKey(clientId)) return;

        int slot = GetFreeSlot();

        if (slot == -1)
        {
            Debug.LogWarning("No hay slots disponibles para el jugador: " + clientId);
            return;
        }

        if (_townSpawnPoints == null || _townSpawnPoints.Length < 4)
        {
            Debug.LogError("NetSpawner necesita cuatro puntos de aparición.");
            return;
        }

        Transform spawn = _townSpawnPoints[slot];

        if (spawn == null)
        {
            Debug.LogError("Falta asignar el punto de aparición del slot: " + slot);
            return;
        }

        _playerSlots.Add(clientId, slot);

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out NetworkClient client))
            return;

        if (client.PlayerObject == null)
            return;

        PlayerDataManager dataManager = PlayerDataManager.Instance;

        if (dataManager == null)
        {
            Debug.LogError("No se encontró PlayerDataManager.");
            return;
        }

        PlayerData data = dataManager.GetOrCreateData(clientId);
        if (data == null) return;

        if (!data.HasCheckpoint)
        {
            data.HasCheckpoint = true;
            data.CheckpointPosition = spawn.position;
            data.CheckpointRotation = spawn.rotation;
        }

        client.PlayerObject.transform.SetPositionAndRotation( data.CheckpointPosition,data.CheckpointRotation);

        Debug.Log("Jugador " + clientId + " asignado al slot " + slot);
    }

    private int GetFreeSlot()
    {
        for (int i = 0; i < 4; i++)
        {
            if (!_playerSlots.ContainsValue(i))
                return i;
        }

        return -1;
    }

    public bool TryGetPlayerSlot(ulong clientId, out int slot)
    {
        return _playerSlots.TryGetValue(clientId, out slot);
    }

    public void RequestRespawn()
    {
        if (!IsClient || !IsSpawned) return;

        RequestRespawnRpc();
    }

    [Rpc(SendTo.Server)]
    private void RequestRespawnRpc(RpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(
            clientId, out NetworkClient client))
            return;

        // Evitar crear un segundo PlayerObject.
        if (client.PlayerObject != null)
        {
            Debug.LogWarning("El jugador todavía tiene un personaje activo.");
            return;
        }

        PlayerDataManager dataManager = PlayerDataManager.Instance;

        if (dataManager == null)
        {
            Debug.LogError("No se encontró PlayerDataManager.");
            return;
        }

        PlayerData data = dataManager.GetOrCreateData(clientId);
        if (data == null) return;

        if (!data.HasCheckpoint)
        {
            Debug.LogError("El jugador no tiene un checkpoint guardado.");
            return;
        }

        GameObject playerPrefab = NetworkManager.Singleton.NetworkConfig.PlayerPrefab;

        if (playerPrefab == null)
        {
            Debug.LogError("No hay Player Prefab asignado en NetworkManager.");
            return;
        }

        // Buscar el diccionario de objetos.
        ItemDictionary dictionary = FindFirstObjectByType<ItemDictionary>();

        if (dictionary == null)
        {
            Debug.LogError("No se encontró ItemDictionary en la escena.");
            return;
        }

        GameObject playerInstance = Instantiate(
            playerPrefab,
            data.CheckpointPosition,
            data.CheckpointRotation
        );

        // Restaurar nivel y experiencia.
        PlayerStats stats = playerInstance.GetComponent<PlayerStats>();
        if (stats != null)
            stats.LoadData(data);

        // Restaurar los tres inventarios.
        Inventory inventory = playerInstance.GetComponent<Inventory>();
        if (inventory != null)
            inventory.RestoreInventory(data, dictionary);

        Inventory2 inventory2 = playerInstance.GetComponent<Inventory2>();
        if (inventory2 != null)
            inventory2.RestoreInventory(data, dictionary);

        Inventory3 inventory3 = playerInstance.GetComponent<Inventory3>();
        if (inventory3 != null)
            inventory3.RestoreInventory(data, dictionary);

        RuenEquip runeEquip = playerInstance.GetComponent<RuenEquip>();
        if(runeEquip != null)
            runeEquip.RestoreRunes(data, dictionary);

        NetworkObject playerNetworkObject =
            playerInstance.GetComponent<NetworkObject>();

        

        if (playerNetworkObject == null)
        {
            Debug.LogError("El Player Prefab no tiene NetworkObject.");
            Destroy(playerInstance);
            return;
        }

        playerNetworkObject.SpawnAsPlayerObject(clientId, true);
        //Restore inv Cliente
        if (inventory != null)
            inventory.SendRestoredInventoryToOwner();

        if (inventory2 != null)
            inventory2.SendRestoredInventoryToOwner();

        if (inventory3 != null)
            inventory3.SendRestoredInventoryToOwner();

        Debug.Log("Jugador " + clientId + " reapareció con sus datos restaurados.");
    }
}
