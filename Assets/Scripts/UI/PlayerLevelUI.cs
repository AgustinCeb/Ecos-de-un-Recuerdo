using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerLevelUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelText;

    private PlayerStats _playerStats;

    private void Update()
    {
        if (_playerStats != null) return;

        if (NetworkManager.Singleton == null) return;

        if (NetworkManager.Singleton.LocalClient?.PlayerObject == null) return;

        _playerStats = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerStats>();

        if (_playerStats == null) return;

        _levelText.text = _playerStats._playerLevel.ToString();

        _playerStats.OnLevelChanged += UpdateLevel;
    }

    private void UpdateLevel(int newLevel)
    {
        _levelText.text = newLevel.ToString();
    }

    private void OnDestroy()
    {
        if (_playerStats != null)
            _playerStats.OnLevelChanged -= UpdateLevel;
    }

}
