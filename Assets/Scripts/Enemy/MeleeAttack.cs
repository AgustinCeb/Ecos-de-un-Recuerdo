using System.Collections;
using UnityEngine;

public class MeleeAttack : MonoBehaviour, IEnemyAttack
{
    [SerializeField]int damageVal;
    [SerializeField] private GameObject attack;
    public void EnemyAttack()
    {
        EnemiesDamage attackdamage=attack.GetComponent<EnemiesDamage>();
        attackdamage.setDamage(damageVal);
        attack.SetActive(true);
        StartCoroutine(offAttack(0.2f));
    }
    IEnumerator offAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        attack.SetActive(false);    
    }
}
