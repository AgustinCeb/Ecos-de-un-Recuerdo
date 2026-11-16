using UnityEngine;
using UnityEngine.UI;


public class PauseMenuUI : MonoBehaviour
{

    public static PauseMenuUI Instance { get; private set; }

    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _exitButton;

    private PauseMenuToggle _playerToggle;

    private void Awake()
    {
        Instance = this;

        _pauseMenu.SetActive(false);

        _resumeButton.onClick.AddListener(ResumeMenu);

        _exitButton.onClick.AddListener(ExitToMenu);
    }

    public void RegisterPlayer(PauseMenuToggle player)
    {
        _playerToggle = player;
    }

    private void ResumeMenu()
    {
        if (_playerToggle == null) return;

        _playerToggle.ResumeMenu();
    }

    public void ExitToMenu()
    {
        if (DeathScreenUI.Instance == null)
        {
            Debug.LogError("No se encontró DeathScreenUI.");
            return;
        }

        DeathScreenUI.Instance.ReturnToMenu();
    }

    public void ShowMenu()
    {
        _pauseMenu.SetActive(true);
    }

    public void HideMenu()
    {
        _pauseMenu.SetActive(false);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
