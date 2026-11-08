using Unity.Netcode;
using UnityEngine;

public class Cofrecontroller : NetworkBehaviour,IPlayerInteractable, IInteractable
{

    [Header("Recompensa")]
    [SerializeField] private ItemData _itemReward;
    [SerializeField] private int _amount = 1;

    [Header("Animacion")]
    [SerializeField] private Animator _animator;

    private NetworkVariable<bool> _isOpened = new NetworkVariable<bool>(
        false,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

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

    [ServerRpc(RequireOwnership = false)]
    private void OpenChestServerRpc(NetworkObjectReference playerReference)
    {
        if (_isOpened.Value) return;

        if (!playerReference.TryGet(out NetworkObject playerObject))
            return;

        if (_itemReward == null)
        {
            Debug.Log("El cofre no tiene recompensa asignada");
            return;
        }

        Inventory inventory = playerObject.GetComponent<Inventory>();
        Inventory2 inventory2 = playerObject.GetComponent<Inventory2>();
        Inventory3 inventory3 = playerObject.GetComponent<Inventory3>();


        switch (_itemReward.Type)
        {
            case ItemData.ItemType.Equipment:

                if (inventory == null) return;

                inventory.addItem(_itemReward, _amount);
                break;

            case ItemData.ItemType.Material:

                if (inventory2 == null) return;

                inventory2.addItem(_itemReward, _amount);
                break;

            case ItemData.ItemType.Consumable:

                if (inventory3 == null) return;

                inventory3.addItem(_itemReward, _amount);
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
        {
            _animator.SetTrigger("Open");
        }
    }

   
}
 