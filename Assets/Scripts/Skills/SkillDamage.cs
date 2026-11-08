using Unity.Netcode;
using UnityEngine;
using System.Collections;

public class SkillDamage : NetworkBehaviour
{

    [SerializeField] private Skilldata skillCastigo;
    [SerializeField] private float _despawnTime = 2f;

    private int _skillDamage;
    private bool _hasHit;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            StartCoroutine(DespawnSkill());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer || _hasHit) return;

        if (other.TryGetComponent<EnemiesHealt>(out EnemiesHealt e))
        {
            _hasHit = true;
            e.TakeDamage(skillCastigo.SkillDamage);
        }
    }

    private IEnumerator DespawnSkill()
    {
        yield return new WaitForSeconds(_despawnTime);

        if (IsSpawned)
        {
            NetworkObject.Despawn();
        }

    }

}
