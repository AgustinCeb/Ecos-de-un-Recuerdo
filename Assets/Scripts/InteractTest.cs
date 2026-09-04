using UnityEngine;

public class InteractTest : MonoBehaviour, IInteractable
{

    public string ActionText => "Hablar";

    public void Interact(GameObject Starter)
    {
        Debug.Log("Hola Capo");

    }

}
