using Unity.Netcode;
using UnityEngine;

public class Cofrecontroller : NetworkBehaviour,IPlayerInteractable, IInteractable
{

    [Header("Recompensa")]
    [SerializeField] private ItemData _itemReward;
    [SerializeField] private int _amount = 1;

    [Header("Animacion")]
    [SerializeField] private Animator _animator;

    private NetworkList<ulong> _openedByPlayers = new();

    public string ActionText
    {
        get
        {
            if (NetworkManager.Singleton == null) return "";

            return _openedByPlayers.Contains(NetworkManager.Singleton.LocalClientId)
                ? ""
                : "Abrir";
        }
    }

    public bool CantInteract()
    {
        return false;
    }

    public override void OnNetworkSpawn()
    {
        _openedByPlayers.OnListChanged += OnChestOpened;
    }

    public override void OnNetworkDespawn()
    {
        _openedByPlayers.OnListChanged -= OnChestOpened;
    }

    public void interact(PlayerInteract player)
    {
        if (!IsSpawned) return;

        if (_openedByPlayers.Contains(NetworkManager.Singleton.LocalClientId))
            return;

        NetworkObject playerNetworkObject = player.GetComponent<NetworkObject>();

        if (playerNetworkObject == null) return;

        OpenChestServerRpc(new NetworkObjectReference(playerNetworkObject));
    }

    public void Interact()
    {
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    private void OpenChestServerRpc(NetworkObjectReference playerReference, RpcParams rpcParams = default)
    {
        ulong senderClientId = rpcParams.Receive.SenderClientId;

        if (_openedByPlayers.Contains(senderClientId))
            return;

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(senderClientId, out NetworkClient client))return;

        if (client.PlayerObject == null) return;

        if (!playerReference.TryGet(out NetworkObject playerObject))
            return;

        if (playerObject != client.PlayerObject)
        {
            Debug.LogWarning("El cofre recibió una referencia de jugador incorrecta.");
            return;
        }


        if (_itemReward == null)
        {
            Debug.Log("El cofre no tiene recompensa asignada");
            return;
        }

        bool rewardGiven = false;

        
        switch (_itemReward.Type)
        {
            case ItemData.ItemType.Equipment:

                Inventory inventory = playerObject.GetComponent<Inventory>();

                if (inventory == null) return;

                inventory.ReceiveChestRewardRpc(_itemReward.ItemID,_amount);
                break;

            case ItemData.ItemType.Material:

                Inventory2 inventory2 = playerObject.GetComponent<Inventory2>();

                if (inventory2 == null) return;

                inventory2.ReceiveChestRewardRpc(_itemReward.ItemID, _amount);
                break;

            case ItemData.ItemType.Consumable:

                Inventory3 inventory3 = playerObject.GetComponent<Inventory3>();

                if (inventory3 == null) return;

                inventory3.ReceiveChestRewardRpc(_itemReward.ItemID, _amount);
                break;

            default:
                Debug.Log("Tipo de objeto no reconocido");
                return;
        }

        _openedByPlayers.Add(senderClientId);
    }

    private void OnChestOpened(NetworkListEvent<ulong> changeEvent)
    {
        if (changeEvent.Type != NetworkListEvent<ulong>.EventType.Add)
            return;

        if (changeEvent.Value == NetworkManager.Singleton.LocalClientId)
            OpenAnimation();
    }

    private void OpenAnimation()
    {
        if (_animator != null)
            _animator.SetTrigger("Open");
    }


}
 