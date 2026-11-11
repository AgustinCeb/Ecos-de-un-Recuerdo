using Unity.VisualScripting;
using UnityEngine;

public class UICloseButton : MonoBehaviour
{
    [SerializeField]GameObject toDeactivate;

    [SerializeField] GameObject _skillUI;

    private PlayertInputState _inputState;

    public void SetInputState(PlayertInputState inputState)
    {
        _inputState = inputState;
    }

    public void closeAction()
    {
        _inputState.SetUIInput(false);

        toDeactivate.SetActive(false);
        _skillUI.SetActive(true);
    }
}
