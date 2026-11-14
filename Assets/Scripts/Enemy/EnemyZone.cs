using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemyZone : MonoBehaviour
{
    [Header("Zone Configuration")]
    [SerializeField] private List<Enemy_Camp> _enemyCamps = new();

    private Dictionary<ulong, int> _playersInside = new();

    private void OnTriggerEnter(Collider other)
    {

        if (!other.CompareTag("Player")) return;

        if (NetworkManager.Singleton == null) return;
        if (!NetworkManager.Singleton.IsServer) return;

        NetworkObject player = other.GetComponentInParent<NetworkObject>();

        if (player == null || !player.IsPlayerObject) return;

        ulong clientId = player.OwnerClientId;

        if (_playersInside.TryGetValue(clientId, out int count))
        {
            _playersInside[clientId] = count + 1;
        }
        else
        {
            _playersInside.Add(clientId, 1);
        }

        if (_playersInside.Count == 1 && count == 0)
        {
            ActivateZone();
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (!other.CompareTag("Player")) return;

        if (NetworkManager.Singleton == null) return;
        if (!NetworkManager.Singleton.IsServer) return;

        NetworkObject player = other.GetComponentInParent<NetworkObject>();

        if (player == null || !player.IsPlayerObject) return;

        ulong clientId = player.OwnerClientId;

        if (!_playersInside.TryGetValue(clientId, out int count)) return;

        
        if (count > 1)
        {
            _playersInside[clientId] = count - 1;
            return;
        }

        _playersInside.Remove(clientId);
       
        if (_playersInside.Count == 0)
        {
            DeactivateZone();
        }
    }

    private void ActivateZone()
    {
        foreach (Enemy_Camp camp in _enemyCamps)
        {
            if (camp == null) continue;

            camp.SpawnEnemies();
        }
    }

    private void DeactivateZone()
    {

        foreach (Enemy_Camp camp in _enemyCamps)
        {
            if (camp == null) continue;
            camp.DespawnEnemies();
        }
    }

    public void ResetZone()
    {
        if (NetworkManager.Singleton == null) return;
        if (!NetworkManager.Singleton.IsServer) return;

        foreach (Enemy_Camp camp in _enemyCamps)
        {
            if (camp == null) continue;

            camp.ResetCamp();
            Debug.Log("Campamento reiniciado: " + camp.name);
        }
    }

}
