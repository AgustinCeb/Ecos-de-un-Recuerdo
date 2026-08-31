using Unity.Netcode;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryToggle : NetworkBehaviour
{

    [SerializeField] private GameObject _inventoryMenu;

    [SerializeField] private InventoryUi _inventoryUi;

    [SerializeField] private PlayerInput _playerInput;
    


    public override void OnNetworkSpawn()
    {
        

        if (!IsOwner) return;

        _inventoryUi = FindFirstObjectByType<InventoryUi>(FindObjectsInactive.Include);
        
        _inventoryMenu = _inventoryUi.transform.gameObject;
        

    }
    public void OnInventory()
    {
        if(!IsOwner) return;

        if (_inventoryMenu == null) return;

        bool active = !_inventoryMenu.activeSelf;
        
        _inventoryMenu.SetActive(active);

        if ( active &&_inventoryMenu != null)
        {
            _inventoryUi.UpdateUI();

        }

        if (active)
        {
            _playerInput.SwitchCurrentActionMap("PlayerUI");
        }

        else
        {
            _playerInput.SwitchCurrentActionMap("Player");
        }

    }

 }
