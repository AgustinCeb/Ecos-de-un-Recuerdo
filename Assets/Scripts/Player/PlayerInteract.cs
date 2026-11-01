using TMPro;
using Unity.Netcode;
using Unity.Services.Matchmaker.Models;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInteract : NetworkBehaviour
{
    [SerializeField] private float _interacRange = 5f;
    [SerializeField] private TextMeshProUGUI _actionText;

    private IInteractable _interactObject;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        SceneManager.sceneLoaded += OnSceneLoaded;

        if (SceneManager.GetActiveScene().name == "Game")
        {
            InitializeUI();
        }

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!IsOwner || scene.name != "Game") return;

        InitializeUI();
    }

    private void InitializeUI()
    {
        GameObject panel = GameObject.Find("Interaction_UI");

        if (panel != null)
        {
            _actionText = panel.GetComponentInChildren<TextMeshProUGUI>(true);
        }

        if (_actionText == null)
            Debug.LogError("No se encontró ActionText en Interaction_UI.");
    }

    private void Update()
    {
        if (!IsOwner) return;

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

        if(!IsOwner) return;

        if (_actionText == null) return;

        _actionText.gameObject.SetActive(_interactObject !=null);

        if(_interactObject != null)
        {
            _actionText.text = _interactObject.ActionText;
        }

    }

    public void OnInteract(InputValue Value)
    {
        if (!IsOwner || _interactObject == null ) return;

        if (_interactObject is IPlayerInteractable playerInteractable)
        {
            playerInteractable.interact(this);
        }
        else
        {
            _interactObject.Interact();
        }

    }

    public override void OnNetworkDespawn()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
