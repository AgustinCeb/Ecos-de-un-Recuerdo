using System;
using Unity.VisualScripting;
using UnityEngine;

public class SkillDamage : MonoBehaviour
{

    [SerializeField] private Skilldata skillCastigo;

    private int _skillDamage;
    

    
    private void OnTriggerEnter(Collider other)
    {
        _skillDamage = skillCastigo.SkillDamage;

        if (other.TryGetComponent<EnemiesHealt>(out EnemiesHealt e))
        {
            e.TakeDamage(_skillDamage);

        }

    }
     

}
