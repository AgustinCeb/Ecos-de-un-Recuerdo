using UnityEngine;
using Unity.Netcode;

public class CastigoData : NetworkBehaviour
{
    [SerializeField] private float _posisionF = 5f;
    [SerializeField] private float _posisionUp = 5f;

    private void Start()
    {
        transform.position += transform.forward * _posisionF;
        transform.position += transform.up * _posisionUp;

    }

    private void OnCollisionEnter(Collision collision)
    {
        bool HitGround = collision.gameObject.layer == LayerMask.NameToLayer("Ground");
        bool HitEnemi = collision.gameObject.CompareTag("Enemy");

        if (HitGround || HitEnemi) 
        {
            if (IsServer)
            {
                NetworkObject.Despawn();
            }
        }
    }

}
