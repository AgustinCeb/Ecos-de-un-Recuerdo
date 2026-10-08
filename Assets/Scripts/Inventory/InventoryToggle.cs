using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryToggle : NetworkBehaviour
{

    [SerializeField] private GameObject _inventoryMenu;

    [SerializeField] private InventoryUi _inventoryUi;

    [SerializeField] private PlayerInput _playerInput;

    [SerializeField] private GameObject _skillUi;


    public override void OnNetworkSpawn()
    {
        

        if (!IsOwner) return;

        _inventoryUi = FindFirstObjectByType<InventoryUi>(FindObjectsInactive.Include);
        
        _inventoryMenu = _inventoryUi.transform.gameObject;

        _skillUi = FindFirstObjectByType<SkillUi>().gameObject;
        

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
            _skillUi.transform.parent.gameObject.SetActive(false);

            
        }

        else
        {
            _playerInput.SwitchCurrentActionMap("Player");
            _skillUi?.transform.parent.gameObject.SetActive(true);
        }

    }

 }
