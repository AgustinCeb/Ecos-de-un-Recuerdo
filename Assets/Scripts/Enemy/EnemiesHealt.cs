using UnityEngine;

public class EnemiesHealt : MonoBehaviour
{

    public int _enemyHealt = 5;

    public void TakeDamage(int damage)
    {
        _enemyHealt -= damage;
        if (_enemyHealt <= 0)
        {
            Destroy(gameObject);

        }
    }

}
