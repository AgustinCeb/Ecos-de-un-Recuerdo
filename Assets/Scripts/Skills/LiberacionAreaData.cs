using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class LiberacionAreaData : MonoBehaviour
{
    [SerializeField] private int _damagePerTic = 5;
    [SerializeField] private float _tickRate = 1f;
    [SerializeField] private float _duration = 10f;

    private void Start()
    {
        StartCoroutine(DamageArea());
        Destroy(gameObject,_duration);

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
