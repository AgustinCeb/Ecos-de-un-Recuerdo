using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] PlayerStats _stats;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemiesHealt>(out EnemiesHealt e))
        {
            e.TakeDamage(_stats.getDamage());

        }
        
    }
}
