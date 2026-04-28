using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryToggle : MonoBehaviour
{
    [SerializeField] private GameObject _inventory;
    [SerializeField] private GameObject _sword;

    public void Start()
    {
        _inventory.SetActive(false);
        _sword.SetActive(false);
    }
    public void OnInventory()
    {
        _inventory.SetActive(!_inventory.activeSelf);
    }

    public void OnAttack()
    {
     
        _sword.SetActive(true);
      
        
    }

}
