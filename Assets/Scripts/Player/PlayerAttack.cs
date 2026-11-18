using Unity.Netcode;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerAttackNetwork _attackNetwork;

    private void Awake()
    {
        _attackNetwork = GetComponentInParent<PlayerAttackNetwork>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_attackNetwork == null) return;

        NetworkObject playerNetworkObject =
            _attackNetwork.GetComponent<NetworkObject>();

        if (playerNetworkObject == null || !playerNetworkObject.IsOwner) return;

        EnemiesHealt enemyHealth = other.GetComponentInParent<EnemiesHealt>();

        if (enemyHealth == null) return;

        NetworkObject enemyNetworkObject =
            enemyHealth.GetComponent<NetworkObject>();

        if (enemyNetworkObject == null || !enemyNetworkObject.IsSpawned) return;

        _attackNetwork.ApplyDamageRpc(
            new NetworkObjectReference(enemyNetworkObject)
        );
    }
}

