using UnityEngine;
using Unity.Netcode;



public class CastigoData : NetworkBehaviour
{
    [SerializeField] private float _posisionF = 5f;
    
    //SFX
    [SerializeField] private AudioClip _skillSound;

    private Transform _direction;

    public void Initialize(Vector3 playerPosition, Vector3 direction)
    {
        direction.Normalize();

        transform.position = playerPosition + direction * _posisionF;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log(
        "RAYO RECIBIDO | Server: " + IsServer +
        " | Client: " + IsClient +
        " | Owner: " + OwnerClientId +
        " | NetworkObjectId: " + NetworkObjectId
    );

        SFXManager.instance.PlaySFX(_skillSound, transform, 1f, 1f);
    }

}
