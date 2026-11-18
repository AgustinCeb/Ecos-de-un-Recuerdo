using Unity.Netcode;
using UnityEngine;

public class PlayerAttackNetwork : NetworkBehaviour
{
    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Owner)]
    public void ApplyDamageRpc(NetworkObjectReference enemyReference)
    {
        if (!IsServer) return;

        if (!enemyReference.TryGet(out NetworkObject enemyObject)) return;

        EnemiesHealt enemyHealth = enemyObject.GetComponent<EnemiesHealt>();

        if (enemyHealth == null) return;

        PlayerStats stats = GetComponent<PlayerStats>();

        if (stats == null) return;

        enemyHealth.TakeDamage(stats.getDamage());
    }

    [Rpc(SendTo.Everyone, InvokePermission = RpcInvokePermission.Owner)]
    public void PlayBasicAttackEffectRpc()
    {
        Attack attack = GetComponentInChildren<Attack>(true);

        if (attack == null) return;

        attack.PlayAttackEffectLocal();
    }
}
