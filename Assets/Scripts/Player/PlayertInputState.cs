using Unity.Netcode;
using UnityEngine.InputSystem;

public class PlayertInputState : NetworkBehaviour
{

    private PlayerInput _playerInput;

    private PlayerCamera _playerCamera;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerCamera = GetComponentInChildren<PlayerCamera>();
    }

    public void SetUIInput(bool uiOpen)
    {
        if (!IsOwner) return;

        if (uiOpen)
        {
            _playerInput.SwitchCurrentActionMap("PlayerUI");
            _playerCamera.SetCameraInput(false);
        }
        else
        {
            _playerInput.SwitchCurrentActionMap("Player");
            _playerCamera.SetCameraInput(true);
        }
    }
}
