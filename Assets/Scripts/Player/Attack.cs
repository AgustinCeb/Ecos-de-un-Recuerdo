using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private GameObject _sword;
    [SerializeField] private GameObject _backSword;
    [SerializeField] private float _attackColdown = 0.3f;

    private void Start()
    {
        _sword.SetActive(false);
        _backSword.SetActive(false);
    }

    private void OnEquip()
    {
        _backSword.SetActive(!_backSword.activeSelf);
    }

    private void OnAttack()
    {
        CancelInvoke(nameof(HideSword));
        _sword.SetActive(true);
        Invoke(nameof(HideSword), _attackColdown);
    }

    private void HideSword()
    {
        _sword.SetActive(false);
    }

}
