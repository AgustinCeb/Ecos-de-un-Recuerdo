using Unity.Netcode;
using UnityEngine;

public class Cofrecontroller : NetworkBehaviour,IPlayerInteractable, IInteractable
{

    [Header("Recompensa")]
    [SerializeField] private ItemData _itemReward;
    [SerializeField] private int _amount = 1;

    [Header("Animacion")]
    [SerializeField] private Animator _animator;

    private NetworkVariable<bool> _isOpened = new NetworkVariable<bool>(false,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server);

    public string ActionText => _isOpened.Value ? "" : "Abrir";

    public bool CantInteract()
    {
        return false;
    }

    public override void OnNetworkSpawn()
    {
        _isOpened.OnValueChanged += OnChestOpened;
    }

    public override void OnNetworkDespawn()
    {
        _isOpened.OnValueChanged -= OnChestOpened;
    }

    public void interact(PlayerInteract player)
    {
        if (_isOpened.Value) return;

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
        if (_isOpened.Value) return;

        ulong senderClientId = rpcParams.Receive.SenderClientId;

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

        _isOpened.Value = true;
    }

    private void OnChestOpened(bool previousValue, bool newValue)
    {
        if (newValue)
            OpenAnimation();
    }

    private void OpenAnimation()
    {
        if (_animator != null)
            _animator.SetTrigger("Open");
    }


}
 