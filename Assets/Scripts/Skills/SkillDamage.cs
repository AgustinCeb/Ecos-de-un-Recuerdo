using System;
using UnityEngine;

public class SkillDamage : MonoBehaviour
{
   

    [SerializeField] CastigoData castigoData;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemiesHealt>(out EnemiesHealt e))
        {
            e.TakeDamage(_castigoData._skillDamage());

        }

    }
     

}
