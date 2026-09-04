using UnityEngine;

public interface IInteractable
{
    string ActionText { get; } // Ej: "Abrir", "Hablar", "Recoger"
    void Interact(GameObject Starter);
}

