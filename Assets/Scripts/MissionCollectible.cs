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

    private NetworkVariable<bool> _isHarvested = new(false);

    private NetworkObject _emptyTreeInstance;

    private Renderer[] _renderers;
    private Collider[] _colliders;

    public string ActionText => "Cosechar";

    private void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>(true);
        _colliders = GetComponentsInChildren<Collider>(true);
    }

    public override void OnNetworkSpawn()
    {
        _isHarvested.OnValueChanged += OnHarvestedChanged;

        ApplyHarvestedState(_isHarvested.Value);
    }

    public override void OnNetworkDespawn()
    {
        _isHarvested.OnValueChanged -= OnHarvestedChanged;
    }

    private void OnHarvestedChanged(bool previousValue, bool newValue)
    {
        ApplyHarvestedState(newValue);
    }

    private void ApplyHarvestedState(bool harvested)
    {
        foreach (Renderer rend in _renderers)
        {
            if (rend != null)
                rend.enabled = !harvested;
        }

        foreach (Collider col in _colliders)
        {
            if (col != null)
                col.enabled = !harvested;
        }
    }

    public bool CantInteract()
    {
        return _isHarvested.Value;
    }

    public void Interact()
    {
    }

    public void interact(PlayerInteract player)
    {
        if (player == null || !player.IsOwner) return;

        if (_isHarvested.Value) return;

        HarvestServerRpc(new NetworkObjectReference(player.NetworkObject));
    }

    [ServerRpc(RequireOwnership = false)]
    private void HarvestServerRpc(
        NetworkObjectReference playerReference,
        ServerRpcParams rpcParams = default)
    {
        if (_isHarvested.Value) return;

        if (_itemData == null || _emptyTreePF == null) return;

        if (!playerReference.TryGet(out NetworkObject playerObject))
            return;

        if (playerObject.OwnerClientId != rpcParams.Receive.SenderClientId)
            return;

        if (Vector3.Distance(transform.position, playerObject.transform.position)
            > _interactDistance)
            return;

        PlayerPickup playerPickup =
            playerObject.GetComponent<PlayerPickup>();

        if (playerPickup == null) return;

        // Crear la versión vacía.
        GameObject emptyTree = Instantiate(
            _emptyTreePF,
            transform.position,
            transform.rotation
        );

        NetworkObject emptyTreeNetwork =
            emptyTree.GetComponent<NetworkObject>();

        if (emptyTreeNetwork == null)
        {
            Destroy(emptyTree);
            Debug.LogError("El prefab vacío no tiene NetworkObject.");
            return;
        }

        emptyTreeNetwork.Spawn();

        _emptyTreeInstance = emptyTreeNetwork;

        // Ocultar el arbusto original para todos.
        _isHarvested.Value = true;

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

        Debug.Log("Arbusto cosechado por: " + playerObject.OwnerClientId);
    }

    [ClientRpc]
    private void GiveItemClientRpc(NetworkObjectReference playerReference, ClientRpcParams clientParams = default)
    {
        if (!playerReference.TryGet(out NetworkObject playerObject))
            return;

        PlayerPickup playerPickup = playerObject.GetComponent<PlayerPickup>();

        if (playerPickup != null)
        {
            playerPickup.GiveItem(_itemData, 1);
        }
    }

    public void ResetCollectible()
    {
        if (!IsServer) return;

        if (!_isHarvested.Value) return;

        // Eliminar la versión vacía.
        if (_emptyTreeInstance != null && _emptyTreeInstance.IsSpawned)
        {
            _emptyTreeInstance.Despawn(true);
        }

        _emptyTreeInstance = null;

        // Mostrar nuevamente el arbusto original.
        _isHarvested.Value = false;

        Debug.Log("Arbusto regenerado: " + gameObject.name);

    }
}