using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField]private ItemData _itemData;
    
    [SerializeField]private int _quantity;

    public ItemData ItemData => _itemData;
    public int Quantity => _quantity;

}
