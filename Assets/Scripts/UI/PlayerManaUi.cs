using UnityEngine;
using Unity.Netcode;
using TMPro;

public class PlayerManaUi : MonoBehaviour
{
    [SerializeField] private TMP_Text _manaText;

    private PlayerMana _playerMana;

    private void Update()
    {
        if (_playerMana != null) return;

        if (NetworkManager.Singleton == null) return;

        if (NetworkManager.Singleton.LocalClient?.PlayerObject == null) return;

        _playerMana = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerMana>();


        _manaText.text = _playerMana.Mana.Value.ToString();

        _playerMana.Mana.OnValueChanged += UpdateHealt;

    }

    private void UpdateHealt(int oldValue, int newValue)
    {
        _manaText.text = newValue.ToString();

    }


}
