using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryContentMenu : MonoBehaviour
{
    [Header("Botones")]
    [SerializeField] private Button _actionButton;
    [SerializeField] private TMP_Text _actionText;
    [SerializeField] private Button _infoButton;
    [SerializeField] private Button _exitButton;

    public void Show(string actionName, Vector2 position)
    {
        transform.position = position;

        _actionText.text = actionName;

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
