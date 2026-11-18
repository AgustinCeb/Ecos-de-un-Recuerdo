using Newtonsoft.Json.Serialization;
using Unity.Netcode;
using UnityEngine;

public class ExpOrb : NetworkBehaviour
{
    [SerializeField] int expValue;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerStats playerStats = other.GetComponent<PlayerStats>();

        if (playerStats == null) return;

        NetworkObject playerNetworkObject = other.GetComponent<NetworkObject>();

        if (playerNetworkObject == null || !playerNetworkObject.IsOwner)
            return;

        playerStats.AddXP(expValue);

        NetworkObject orbNetworkObject = GetComponent<NetworkObject>();

        if (orbNetworkObject != null)
        {
            DespawnOrbServerRpc(orbNetworkObject.NetworkObjectId);
        }

    }

    [ServerRpc(RequireOwnership = false)]
    private void DespawnOrbServerRpc(ulong orbId)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(
            orbId, out NetworkObject orb))
        {
            orb.Despawn();
        }
    }

}
