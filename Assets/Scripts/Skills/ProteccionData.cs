using Unity.Netcode;
using UnityEngine;

public class ProteccionData : NetworkBehaviour
{
    
    private PlayerHealt _playerHealt;

    public void SetOwner(PlayerHealt playerHealt)
    {
        _playerHealt = playerHealt;

        Debug.Log("SetOwner ejecutado: " + (_playerHealt != null));
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        Debug.Log(
        "Escudo recibido | Server: " + IsServer +
        " | Client: " + IsClient +
        " | Owner: " + OwnerClientId
    );

        if (_playerHealt == null)
        {
            Debug.LogError("No se asignó PlayerHealt al escudo.");
            NetworkObject.Despawn();
            return;
        }

        if (_playerHealt.ShieldActivate.Value)
        {
            Debug.Log("Escudo ya activado");
            NetworkObject.Despawn();
            return;
        }

        _playerHealt.ShieldActivate.Value = true;
        _playerHealt.ShieldHits.Value = 3;
    }

    private void Update()
    {
        if (!IsServer || !IsSpawned) return;
        if (_playerHealt == null) return;

        if (!_playerHealt.ShieldActivate.Value)
            NetworkObject.Despawn();
    }
}
