using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class LiberacionAreaData : NetworkBehaviour
{
    [SerializeField] private int _damagePerTic = 5;
    [SerializeField] private float _tickRate = 1f;
    [SerializeField] private float _duration = 10f;

    private void Start()
    {
        if (IsServer)
        {
            
            StartCoroutine(DamageArea());
            Invoke(nameof(DestruirObjetoRed), _duration);
        }

    }

    private void DestruirObjetoRed()
    {
        if (NetworkObject != null && NetworkObject.IsSpawned)
        {
            NetworkObject.Despawn(true); 
        }
    }

    private IEnumerator DamageArea()
    {
        while (true)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, 5f);

            foreach (Collider Hit in hits) 
            {
                if (Hit.CompareTag("Enemy"))
                {
                    Hit.GetComponent<EnemiesHealt>()?.TakeDamage(_damagePerTic);
                }

            }

            yield return new WaitForSeconds(_tickRate);

        }

    }
}
