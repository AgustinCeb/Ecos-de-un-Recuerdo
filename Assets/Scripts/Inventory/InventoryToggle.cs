using Unity.Netcode;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryToggle : NetworkBehaviour
{

    [SerializeField] private GameObject _inventory;

    [SerializeField] private InventoryUi _inventoryUi;

    [SerializeField] private PlayerInput _playerInput;
    


    public void Start()
    {
        if (!IsOwner) return;

        _inventory = GameObject.Find("PNL_Inventori");

        _inventoryUi = FindFirstObjectByType<InventoryUi>();
        

        _inventory.SetActive(false);
        
    }
    public void OnInventory()
    {
        if(!IsOwner) return;

        if (_inventory == null) return;

        bool active = !_inventory.activeSelf;
        
        _inventory.SetActive(active);

        if ( active &&_inventory != null)
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
    



    


