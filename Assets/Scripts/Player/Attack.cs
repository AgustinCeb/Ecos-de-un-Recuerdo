using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private GameObject _sword;
    
    [SerializeField] private float _attackColdown = 0.3f;

    //SFX
    [SerializeField] private AudioClip _attackSound;

    private void Start()
    {
        _sword.SetActive(false);
    }

   
    private void OnAttack()
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
