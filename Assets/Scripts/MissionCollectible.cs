using System;
using Unity.Netcode;
using UnityEngine;

public class MissionCollectible : NetworkBehaviour, IPlayerInteractable, IInteractable
{
    [Header("Objeto")]
    [SerializeField] private ItemData _itemData;
    [SerializeField] private GameObject _emptyTreePF;

    [Header("Configuración")]
    [SerializeField] private float _interactDistance = 6f;


    public bool _isHarvested;

    public string ActionText => "Cosechar";


    public bool CantInteract()
    {
        return false;
    }

    public void Interact()
    {
        
    }


    public void interact(PlayerInteract player)
    {
        if (player == null || !player.IsOwner) return;

        HarvestServerRpc(new NetworkObjectReference(player.NetworkObject));
    }

    [ServerRpc(RequireOwnership = false)]
    private void HarvestServerRpc(
        NetworkObjectReference playerReference,
        ServerRpcParams rpcParams = default)
    {
        if (_isHarvested) return;

        if (_itemData == null || _emptyTreePF == null) return;

        // Comprobar que el jugador exista.
        if (!playerReference.TryGet(out NetworkObject playerObject))
            return;

        // Comprobar que sea el jugador que envió la solicitud.
        if (playerObject.OwnerClientId != rpcParams.Receive.SenderClientId)
            return;

        // Comprobar la distancia.
        if (Vector3.Distance(transform.position, playerObject.transform.position)
            > _interactDistance)
            return;

        PlayerPickup playerPickup =
            playerObject.GetComponent<PlayerPickup>();

        if (playerPickup == null) return;

        _isHarvested = true;

        // Entregar el objeto solamente al jugador que cosechó.
        ClientRpcParams clientParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[]
                {
                    playerObject.OwnerClientId
                }
            }
        };

        GiveItemClientRpc(playerReference, clientParams);

        // Crear el árbol sin hojas.
        GameObject emptyTree = Instantiate(_emptyTreePF, transform.position, transform.rotation);

        NetworkObject emptyTreeNetwork = emptyTree.GetComponent<NetworkObject>();

        if (emptyTreeNetwork == null)
        {
            Destroy(emptyTree);
            _isHarvested = false;
            return;
        }

        emptyTreeNetwork.Spawn();

        // Eliminar el árbol original.
        NetworkObject.Despawn(emptyTree);
    }

    [ClientRpc]
    private void GiveItemClientRpc(
        NetworkObjectReference playerReference,
        ClientRpcParams clientParams = default)
    {
        if (!playerReference.TryGet(out NetworkObject playerObject))
            return;

        PlayerPickup playerPickup = playerObject.GetComponent<PlayerPickup>();

        if (playerPickup != null)
        {
            playerPickup.GiveItem(_itemData, 1);
        }
    }

    
}
