using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenUI : MonoBehaviour
{
    public static DeathScreenUI Instance { get; private set; }

    [Header("Death Screen")]
    [SerializeField] private GameObject _deathPanel;

    private bool _returningToMenu;

    private void Awake()
    {
        Instance = this;
        _deathPanel.SetActive(false);
    }

    private void Start()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;
        }
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnected;
        }

        if (Instance == this)
            Instance = null;
    }

    public void ShowDeathScreen()
    {
        _deathPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HideDeathScreen()
    {
        _deathPanel.SetActive(false);
    }

    public void ReturnToMenu()
    {
        if (_returningToMenu) return;

        _returningToMenu = true;

        NetworkManager networkManager = NetworkManager.Singleton;

        if (networkManager != null &&
            (networkManager.IsClient || networkManager.IsServer))
        {
            networkManager.Shutdown();
        }

        SceneManager.LoadScene("MainMenu");
    }

    private void HandleClientDisconnected(ulong clientId)
    {
        NetworkManager networkManager = NetworkManager.Singleton;

        if (networkManager == null) return;

        // Solo reaccionar cuando se desconecta este cliente.
        if (clientId != networkManager.LocalClientId) return;

        // El host vuelve al menú desde su propio botón.
        if (networkManager.IsServer) return;

        ReturnToMenu();
    }

}
