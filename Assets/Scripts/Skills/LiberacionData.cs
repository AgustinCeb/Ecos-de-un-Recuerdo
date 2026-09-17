using UnityEngine;
using Unity.Netcode;

public class LiberacionData : NetworkBehaviour
{

    [SerializeField] private Skilldata _skillUlti;

    [SerializeField] private GameObject _areaPF;

    private int _skillDamage;

    private void Start()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 5f);

        _skillDamage = _skillUlti.SkillDamage;

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                hit.GetComponent<EnemiesHealt>()?.TakeDamage(_skillDamage);

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
