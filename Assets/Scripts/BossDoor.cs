using Unity.Netcode;
using UnityEngine;

public class BossDoor : NetworkBehaviour,IPlayerInteractable , IInteractable
{

    [Header("Objeto requerido")]
    [SerializeField] private ItemDictionary _itemDictionary;
    [SerializeField] private int _requiredItemID;
    [SerializeField] private int _requiredAmount = 1;

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
        _isOpened.OnValueChanged += OnDoorOpened;
    }

    public override void OnNetworkDespawn()
    {
        _isOpened.OnValueChanged -= OnDoorOpened;
    }

    public void interact(PlayerInteract player)
    {
        if (_isOpened.Value) return;

        NetworkObject playerNetworkObject = player.GetComponent<NetworkObject>();

        if (playerNetworkObject == null) return;

        OpenDoorServerRpc(new NetworkObjectReference(playerNetworkObject));
    }

    public void Interact()
    {
    }

    [ServerRpc(RequireOwnership = false)]
    private void OpenDoorServerRpc(NetworkObjectReference playerReference)
    {
        if (_isOpened.Value) return;

        if (!playerReference.TryGet(out NetworkObject playerObject))
            return;

        if (_itemDictionary == null)
        {
            Debug.LogWarning("No se asignó el ItemDictionary");
            return;
        }

        // Obtener el objeto requerido desde el diccionario
        ItemData requiredItem = _itemDictionary.GetItem(_requiredItemID);

        if (requiredItem == null) return;

        // Obtener inventario del jugador
        Inventory2 inventory2 = playerObject.GetComponent<Inventory2>();

        if (inventory2 == null) return;

        // Comprobar cantidad disponible
        int amountInInventory = inventory2.GetItemCount(requiredItem);

        if (amountInInventory < _requiredAmount)
        {
            Debug.Log("No tienes el objeto necesario para abrir la puerta");
            GameManager.Instance.ShowNotification("No se Abre Requiere una Llave Especial");
            return;
        }

        // Consumir el objeto
        bool removed = inventory2.RemoveItem(requiredItem, _requiredAmount);

        if (!removed)
        {
            Debug.Log("No se pudo consumir el objeto requerido");
            return;
        }

        GameManager.Instance.ShowNotification("Se Uso Sello Final");

        // Abrir puerta
        _isOpened.Value = true;
    }

    private void OnDoorOpened(bool previousValue, bool newValue)
    {
        if (newValue)
        {
            _animator.SetTrigger("Open");
            _animator.SetFloat("Door1", 1f);
            _animator.SetFloat("Door2", 1f);
        }
    }
}
