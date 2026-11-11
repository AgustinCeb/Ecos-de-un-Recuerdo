using System;
using Unity.VisualScripting;
using UnityEngine;

public class CraftingNPC : MonoBehaviour, IInteractable, IPlayerInteractable
{
    public string ActionText => "Craftear";
    [SerializeField] private GameObject menu;

    [SerializeField] private GameObject _skillUI;

    private PlayertInputState _inputState;

    public bool CantInteract()
    {
        return false;
    }

    public void Interact()
    {
       
    }

    public void interact(PlayerInteract player)
    {
        _inputState = player.GetComponent<PlayertInputState>();

        menu.GetComponentInChildren<UICloseButton>().SetInputState(_inputState);

        _inputState.SetUIInput(true);

        CraftingManager crafting = menu.GetComponent<CraftingManager>();

        crafting.player = player.gameObject;

        menu.SetActive(true);
        _skillUI.SetActive(false);
    }

}
