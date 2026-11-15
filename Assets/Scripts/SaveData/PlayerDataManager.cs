using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;

    private Dictionary<ulong, PlayerData> _playersData = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public PlayerData GetOrCreateData(ulong clientId)
    {
        if (NetworkManager.Singleton == null ||!NetworkManager.Singleton.IsServer)return null;

        if (!_playersData.TryGetValue(clientId, out PlayerData data))
        {
            data = new PlayerData();
            _playersData.Add(clientId, data);

            Debug.Log("Datos creados para el jugador: " + clientId);
        }

        return data;
    }

    public bool TryGetData(ulong clientId, out PlayerData data)
    {
        data = null;

        if (NetworkManager.Singleton == null ||!NetworkManager.Singleton.IsServer)return false;

        return _playersData.TryGetValue(clientId, out data);
    }

    public void RemovePlayerData(ulong clientId)
    {
        if (NetworkManager.Singleton == null ||!NetworkManager.Singleton.IsServer)return;

        if (_playersData.Remove(clientId))
        {
            Debug.Log("Datos eliminados del jugador: " + clientId);
        }
    }

    public void ClearAllData()
    {
        if (NetworkManager.Singleton == null ||!NetworkManager.Singleton.IsServer)return;

        _playersData.Clear();

        Debug.Log("Todos los datos de los jugadores fueron eliminados.");
    }

}
