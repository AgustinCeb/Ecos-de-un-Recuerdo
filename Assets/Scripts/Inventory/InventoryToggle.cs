using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InventoryToggle : NetworkBehaviour
{

    [SerializeField] private GameObject _inventoryMenu;

    [SerializeField] private InventoryUi _inventoryUi;

    [SerializeField] private PlayerInput _playerInput;

    [SerializeField] private GameObject _skillUi;


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
        _inventoryUi = FindFirstObjectByType<InventoryUi>(FindObjectsInactive.Include);

        SkillUi skill = FindFirstObjectByType<SkillUi>(FindObjectsInactive.Include);

        if (_inventoryUi != null)
            _inventoryMenu = _inventoryUi.gameObject;

        if (skill != null)
            _skillUi = skill.gameObject;

        if (_inventoryUi == null)
            Debug.LogError("No se encontró InventoryUi en Game.");

        if (_skillUi == null)
            Debug.LogError("No se encontró SkillUi en Game.");
    }

    public void OnInventory()
    {
        if(!IsOwner|| _inventoryMenu == null) return;

        bool active = !_inventoryMenu.activeSelf;
        
        _inventoryMenu.SetActive(active);

        if (active)
        {

            _inventoryUi.UpdateUI();
            _playerInput.SwitchCurrentActionMap("PlayerUI");
            _skillUi.transform.parent.gameObject.SetActive(false);

            
        }

        else
        {
            _playerInput.SwitchCurrentActionMap("Player");
            _skillUi?.transform.parent.gameObject.SetActive(true);
        }

    }

    public override void OnNetworkDespawn()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
