public interface IInteractable
{
    string ActionText { get; } // Ej: "Abrir", "Hablar", "Recoger"
    bool CantInteract();
    void Interact();
}

