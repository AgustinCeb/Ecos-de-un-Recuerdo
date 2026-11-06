using UnityEngine;
using Unity.Netcode;

public class LiberacionData : NetworkBehaviour
{

    [SerializeField] private Skilldata _skillUlti;

    [SerializeField] private GameObject _areaPF;

    private int _skillDamage;

    private NetworkObject _playerObject;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, 5f);

        _skillDamage = _skillUlti.SkillDamage;

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                hit.GetComponent<EnemiesHealt>()?.TakeDamage(_skillDamage);

            }
        }

        StartCoroutine(EsperarActivacion());

    }

    private System.Collections.IEnumerator EsperarActivacion()
    {
        yield return new WaitForSeconds(1f);

        _playerObject = transform.parent.GetComponent<NetworkObject>();

        CreateArea();

        NetworkObject.Despawn();
    }

    private void CreateArea()
    {
        GameObject area = Instantiate(_areaPF,transform.position,Quaternion.identity);
        
        NetworkObject areaObject = area.GetComponent<NetworkObject>();

        areaObject.Spawn();

        areaObject.TrySetParent(_playerObject, false);

        area.transform.localPosition = Vector3.zero;
        area.transform.localRotation = Quaternion.identity;

    }

}
