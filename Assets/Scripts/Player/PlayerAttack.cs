using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private int _damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemiesHealt>(out EnemiesHealt e))
        {
            e.TakeDamage(_damage);

        }
        
    }
}
