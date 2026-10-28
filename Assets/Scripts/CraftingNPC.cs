using UnityEngine;

public class CraftingNPC : MonoBehaviour, IInteractable
{
    public string ActionText => "Craftear";
    [SerializeField] private GameObject menu;

    public void Interact(GameObject Starter)
    {
        if (Starter.transform.CompareTag("Player"))
        {
            menu.SetActive(true);
            menu.GetComponent<CraftingManager>().player = Starter;  
        }
    }

    public bool CantInteract()
    {
        throw new System.NotImplementedException();
    }

    public void Interact()
    {
        throw new System.NotImplementedException();
    }
}
