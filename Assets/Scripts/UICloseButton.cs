using UnityEngine;

public class UICloseButton : MonoBehaviour
{
    [SerializeField]GameObject toDeactivate;

    public void closeAction()
    {
        toDeactivate.SetActive(false);
    }
}
