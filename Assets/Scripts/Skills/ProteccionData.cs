using Unity.Netcode;
using UnityEngine;

public class ProteccionData : MonoBehaviour
{
    private PlayerHealt _playerHealt;

    public void SetOwner(PlayerHealt playerHealt)
    {
        _playerHealt = playerHealt;
    }
    
    private void Start()
    {
        if (_playerHealt == null)
        {
            Debug.Log("No se Asigno playerHealt");
            return;
        }


        if (_playerHealt.ShieldActivate.Value)
        {
            Debug.Log("Escudo Ya activado");
            GetComponent<NetworkObject>().Despawn();
            return;
        }

        _playerHealt.ShieldActivate.Value = true;
        _playerHealt.ShieldHits.Value = 3;

        

    }

    private void Update()
    {
        if (_playerHealt == null) return;

        if (!_playerHealt.ShieldActivate.Value)
        {
            GetComponent<NetworkObject>().Despawn();
        }

    }


}
