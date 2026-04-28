using Newtonsoft.Json.Serialization;
using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    [SerializeField] int expValue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats.Instance.AddXP(expValue);
            Destroy(gameObject);    
        }
    }
}
