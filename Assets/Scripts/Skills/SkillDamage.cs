using System;
using UnityEngine;

public class SkillDamage : MonoBehaviour
{
   

    

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemiesHealt>(out EnemiesHealt e))
        {
            

        }

    }
     

}
