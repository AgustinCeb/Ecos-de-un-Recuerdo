using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryToggle : MonoBehaviour
{

    [SerializeField] private GameObject _inventory;
    


    public void Start()
    {
        _inventory.SetActive(false);
        
    }
    public void OnInventory()
    {
        _inventory.SetActive(!_inventory.activeSelf);
    
    }




}   


