using UnityEngine;
using Unity.Netcode;
using TMPro;

public class PlayerShieldHitUI : MonoBehaviour
{

    [SerializeField] private TMP_Text _shieldText;

    private PlayerHealt _playerHealt;

    private void Update()
    {
        if (_playerHealt != null) return;

        if (NetworkManager.Singleton == null) return;

        if (NetworkManager.Singleton.LocalClient?.PlayerObject == null) return;

        _playerHealt = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerHealt>();


        _shieldText.text = _playerHealt.ShieldHits.Value.ToString();

        _playerHealt.ShieldHits.OnValueChanged += UpdateShield;

    }

    private void UpdateShield(int oldValue, int newValue)
    {
        _shieldText.text = newValue.ToString();

    }
}
