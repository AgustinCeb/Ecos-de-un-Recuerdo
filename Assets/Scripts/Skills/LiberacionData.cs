using UnityEngine;
using Unity.Netcode;

public class LiberacionData : NetworkBehaviour
{
    [SerializeField] private int _damageExplotion = 30;

    [SerializeField] private GameObject _areaPF;

    private void Start()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 5f);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                hit.GetComponent<EnemiesHealt>()?.TakeDamage(_damageExplotion);

            }
        }

        CreateArea();

        NetworkObject.Despawn();

    }

    private void CreateArea()
    {
        GameObject area = Instantiate(_areaPF,transform.position,Quaternion.identity);
        area.GetComponent<NetworkObject>().Spawn();
    }

}
