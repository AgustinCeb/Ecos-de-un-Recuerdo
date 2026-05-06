using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryToggle : NetworkBehaviour
{

    [SerializeField] private GameObject _inventory;
    


    public void Start()
    {
        if (!IsOwner) return;

        _inventory = GameObject.Find("PNL_Inventori");

        _inventory.SetActive(false);
        
    }
    public void OnInventory()
    {
        if(!IsOwner) return;

        _inventory.SetActive(!_inventory.activeSelf);
    
    }




}   


