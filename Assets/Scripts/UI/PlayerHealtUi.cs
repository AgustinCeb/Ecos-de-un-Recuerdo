using UnityEngine;
using Unity.Netcode;
using TMPro;

public class PlayerHealtUi : MonoBehaviour
{

    [SerializeField] private TMP_Text _healtText;

    private PlayerHealt _playerHealt;

    private void Update()
    {
        if(_playerHealt != null) return;

        if (NetworkManager.Singleton == null) return;

        if (NetworkManager.Singleton.LocalClient?.PlayerObject == null) return;

        _playerHealt = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerHealt>();


        _healtText.text = _playerHealt.Healt.Value.ToString();

        _playerHealt.Healt.OnValueChanged += UpdateHealt;

    }

    private void UpdateHealt(int oldValue, int newValue)
    {
        _healtText.text = newValue.ToString();

    }

}
