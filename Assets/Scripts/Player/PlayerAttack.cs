using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private int _damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<EnemiesHealt>(out EnemiesHealt e))
        {
            e.TakeDamage(_damage);

        }
        
    }
}
