using Unity.Netcode;
using UnityEngine;

public class TutorialDoor : NetworkBehaviour, IPlayerInteractable, IInteractable
{

    [Header("Objeto requerido")]
    [SerializeField] private ItemDictionary _itemDictionary;
    [SerializeField] private int _requiredItemID;
    [SerializeField] private int _requiredAmount = 1;

    public string ActionText => "Cortar";

    public bool CantInteract()
    {
        return false;
    }

    
    public void interact(PlayerInteract player)
    {
        
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
            GameManager.Instance.ShowNotification("No se puede Atravezar");
            return;
        }

        // Consumir el objeto
        bool removed = inventory2.RemoveItem(requiredItem, _requiredAmount);

        if (!removed)
        {
            Debug.Log("No se pudo consumir el objeto requerido");
            return;
        }

        GameManager.Instance.ShowNotification("El Hacha se rompio tras su uso");

        // Despawnear objeto
        NetworkObject.Despawn();

        
    }

}
