using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using Unity.Services.Matchmaker.Models;

public class PlayerInteract : NetworkBehaviour
{
    [SerializeField] private float _interacRange = 5f;
    [SerializeField] private TextMeshProUGUI _actionText;

    private IInteractable _interactObject;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        GameObject panel = GameObject.Find("Interaction_UI");

        if (panel != null)
        {
            _actionText = panel.GetComponentInChildren<TextMeshProUGUI>();
        }

    }

    private void Update()
    {
        Collider[] CloserObj = Physics.OverlapSphere(transform.position, _interacRange);

        _interactObject = null;

        foreach(var col in CloserObj)
        {
            var interactable = col.GetComponent<IInteractable>();
            if(interactable != null)
            {
                _interactObject = interactable;
                break;
            }
        }

        _actionText.gameObject.SetActive(_interactObject !=null);
        if(_interactObject != null)
        {
            _actionText.text = _interactObject.ActionText;
        }

    }

    public void OnInteract(InputValue Value)
    {
        
        if (_interactObject != null) 
        {
            _interactObject.Interact(gameObject);
        }

    }

}
