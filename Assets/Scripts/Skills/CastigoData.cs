using UnityEngine;
using Unity.Netcode;


public class CastigoData : NetworkBehaviour
{
    [SerializeField] private float _posisionF = 5f;
    [SerializeField] private float _posisionUp = 5f;
    [SerializeField] private float _fallVelocity = 25f;

    //SFX
    [SerializeField] private AudioClip _skillSound;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.linearVelocity = Vector3.down * _fallVelocity;

        transform.position += transform.forward * _posisionF;
        transform.position += transform.up * _posisionUp;
        SFXManager.instance.PlaySFX(_skillSound, transform, 1f, 1f);

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
