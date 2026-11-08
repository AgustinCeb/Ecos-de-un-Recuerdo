using UnityEngine;
using Unity.Netcode;



public class CastigoData : NetworkBehaviour
{
    [SerializeField] private float _posisionF = 5f;
    
    //SFX
    [SerializeField] private AudioClip _skillSound;

    private Transform _direction;

    public void Initialize(PlayerMove playerMove)
    {
        Transform direction = playerMove.Direction;

        transform.position = playerMove.transform.position
            + direction.forward * _posisionF;

        transform.rotation = direction.rotation;
    }

    public override void OnNetworkSpawn()
    {
        SFXManager.instance.PlaySFX(_skillSound, transform, 1f, 1f);
    }

}
