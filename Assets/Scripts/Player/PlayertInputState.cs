using Unity.Netcode;
using UnityEngine.InputSystem;

public class PlayertInputState : NetworkBehaviour
{

    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    public void SetUIInput(bool uiOpen)
    {
        if (!IsOwner) return;

        if (uiOpen)
        {
            _playerInput.SwitchCurrentActionMap("PlayerUI");
        }
        else
        {
            _playerInput.SwitchCurrentActionMap("Player");
        }
    }
}
