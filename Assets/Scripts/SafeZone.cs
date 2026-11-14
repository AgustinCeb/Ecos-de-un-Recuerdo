using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SafeZone : NetworkBehaviour
{
    [Header("Regeneration")]
    [SerializeField] private int _healthPerSecond = 5;
    [SerializeField] private int _manaPerSecond = 5;

    [SerializeField] private List<EnemyZone> _enemyZones = new();

    private bool _campsResetForCurrentVisit;

    private class PlayerRecovery
    {
        public NetworkObject NetworkObject;
        public PlayerHealt Health;
        public PlayerMana Mana;
        public float Timer;
    }

    private Dictionary<ulong, PlayerRecovery> _playersInside = new();

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;
        if (!other.CompareTag("Player")) return;

        NetworkObject player = other.GetComponentInParent<NetworkObject>();

        if (player == null || !player.IsPlayerObject) return;

        ulong clientId = player.OwnerClientId;

        if (_playersInside.ContainsKey(clientId)) return;

        PlayerHealt health = player.GetComponent<PlayerHealt>();
        PlayerMana mana = player.GetComponent<PlayerMana>();

        if (health == null || mana == null) return;

        _playersInside.Add(clientId, new PlayerRecovery
        {
            NetworkObject = player,
            Health = health,
            Mana = mana,
            Timer = 0f
        });

        CheckAllPlayersInside();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsServer) return;
        if (!other.CompareTag("Player")) return;

        NetworkObject player = other.GetComponentInParent<NetworkObject>();

        if (player == null || !player.IsPlayerObject) return;

        _playersInside.Remove(player.OwnerClientId);

        _campsResetForCurrentVisit = false;

    }

    private void Update()
    {
        if (!IsServer) return;

        List<ulong> playersToRemove = new();

        foreach (var entry in _playersInside)
        {
            PlayerRecovery recovery = entry.Value;

            if (recovery.NetworkObject == null ||
                !recovery.NetworkObject.IsSpawned)
            {
                playersToRemove.Add(entry.Key);
                continue;
            }

            recovery.Timer += Time.deltaTime;

            if (recovery.Timer >= 1f)
            {
                recovery.Timer -= 1f;

                recovery.Health.HealPlayer(_healthPerSecond);
                recovery.Mana.RestoreMana(_manaPerSecond);
            }
        }

        foreach (ulong clientId in playersToRemove)
        {
            _playersInside.Remove(clientId);
        }
    }

    private void CheckAllPlayersInside()
    {
        int connectedPlayers = NetworkManager.Singleton.ConnectedClientsIds.Count;

        if (connectedPlayers == 0) return;

        if (_playersInside.Count < connectedPlayers) return;

        if (_campsResetForCurrentVisit) return;

        foreach (EnemyZone zone in _enemyZones)
        {
            if (zone == null) continue;

            zone.ResetZone();
        }

        _campsResetForCurrentVisit = true;

        Debug.Log("Todos los jugadores regresaron al pueblo. Zonas reiniciadas.");
    }

}
