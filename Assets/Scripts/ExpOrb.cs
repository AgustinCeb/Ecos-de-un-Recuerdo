using Newtonsoft.Json.Serialization;
using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    [SerializeField] int expValue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!other.CompareTag("Player"))
                return;

            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            if (playerStats == null)
                return;

            playerStats.AddXP(expValue);

            Destroy(gameObject);    
        }
    }
}
