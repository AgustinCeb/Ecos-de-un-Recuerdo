using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using UnityEngine.UI;

public class PauseMenuToggle : NetworkBehaviour
{
    
    private PlayertInputState _inputState;

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
        _inputState = GetComponent<PlayertInputState>();

        if (PauseMenuUI.Instance == null)
        {
            Debug.LogError("No se encontró PauseMenuUI en Game.");
            return;
        }

        PauseMenuUI.Instance.RegisterPlayer(this);
    }

    public void OnMenu()
    {
        if (!IsOwner || PauseMenuUI.Instance == null) return;

        PauseMenuUI.Instance.ShowMenu();
        _inputState.SetUIInput(true);
    }

    public void OnMenuOn()
    {
        ResumeMenu();
    }

    public void ResumeMenu()
    {
        if (!IsOwner || PauseMenuUI.Instance == null) return;

        ResumeMenuInternal();
    }

    private void ResumeMenuInternal()
    {
        PauseMenuUI.Instance.HideMenu();
        _inputState.SetUIInput(false);
    }

    public override void OnNetworkDespawn()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
