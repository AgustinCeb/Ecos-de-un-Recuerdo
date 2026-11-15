using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public static PlayerSpawn Instance { get; private set; }

    [Header("Spawn")]
    [SerializeField] private Transform _defaultSpawn;

    private readonly HashSet<ulong> _respawningPlayers = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        }

        if (Instance == this)
            Instance = null;
    }

    private void OnClientDisconnected(ulong clientId)
    {
        if (NetworkManager.Singleton == null) return;
        if (!NetworkManager.Singleton.IsServer) return;

        PlayerDataManager.Instance?.RemovePlayerData(clientId);

        _respawningPlayers.Remove(clientId);
    }

    public void RespawnPlayer(ulong clientId)
    {
        if (NetworkManager.Singleton == null) return;
        if (!NetworkManager.Singleton.IsServer) return;

        if (!_respawningPlayers.Add(clientId)) return;

        StartCoroutine(RespawnNextFrame(clientId));
    }

    private IEnumerator RespawnNextFrame(ulong clientId)
    {
        yield return null;

        if (NetworkManager.Singleton == null ||
            !NetworkManager.Singleton.IsServer)
        {
            _respawningPlayers.Remove(clientId);
            yield break;
        }

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(
            clientId, out NetworkClient client))
        {
            _respawningPlayers.Remove(clientId);
            yield break;
        }

        if (client.PlayerObject != null)
        {
            Debug.LogWarning("El jugador todavía tiene un objeto activo.");
            _respawningPlayers.Remove(clientId);
            yield break;
        }

        if (_defaultSpawn == null)
        {
            Debug.LogError("No se asignó Spawn_Default.");
            _respawningPlayers.Remove(clientId);
            yield break;
        }

        Vector3 spawnPosition = _defaultSpawn.position;
        Quaternion spawnRotation = _defaultSpawn.rotation;

        if (PlayerDataManager.Instance != null &&
            PlayerDataManager.Instance.TryGetData(clientId, out PlayerData data) &&
            data.HasCheckpoint)
        {
            spawnPosition = data.CheckpointPosition;
            spawnRotation = data.CheckpointRotation;
        }

        GameObject playerPrefab =
            NetworkManager.Singleton.NetworkConfig.PlayerPrefab;

        if (playerPrefab == null)
        {
            Debug.LogError("No hay Player Prefab asignado en NetworkManager.");
            _respawningPlayers.Remove(clientId);
            yield break;
        }

        GameObject playerInstance = Instantiate(
            playerPrefab,
            spawnPosition,
            spawnRotation
        );

        NetworkObject newPlayer =
            playerInstance.GetComponent<NetworkObject>();

        if (newPlayer == null)
        {
            Debug.LogError("El Player Prefab no tiene NetworkObject.");
            Destroy(playerInstance);
            _respawningPlayers.Remove(clientId);
            yield break;
        }

        newPlayer.SpawnAsPlayerObject(clientId);

        Debug.Log("Jugador reaparecido: " + clientId);

        _respawningPlayers.Remove(clientId);
    }

}
