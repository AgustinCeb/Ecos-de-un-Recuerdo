using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private GameObject _sword;
    
    [SerializeField] private float _attackColdown = 0.3f;

    //SFX
    [SerializeField] private AudioClip _attackSound;

    [SerializeField] private Animator _animator;

    private void Start()
    {
        _sword.SetActive(false);
    }

   
    public void OnAttack()
    {
        _animator.SetTrigger("Attack");

    }

    public void PlayerAttack()
    {
        CancelInvoke(nameof(HideSword));
        SFXManager.instance.PlaySFX(_attackSound, transform, 1f, 1f);
        _sword.SetActive(true);
        Invoke(nameof(HideSword), _attackColdown);
    }

    private void HideSword()
    {
        _sword.SetActive(false);
    }

}
