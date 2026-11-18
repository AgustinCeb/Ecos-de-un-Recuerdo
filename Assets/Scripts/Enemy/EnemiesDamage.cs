using Unity.Netcode;
using UnityEngine;

public class EnemiesDamage : MonoBehaviour
{
    [SerializeField] private int _damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (NetworkManager.Singleton == null) return;
        if (!NetworkManager.Singleton.IsServer) return;

        PlayerHealt playerHealth = other.GetComponentInParent<PlayerHealt>();

        if (playerHealth == null) return;

        playerHealth.TakeDamagePlayer(_damage);
    }

    public void setDamage(int damage)
    {
        _damage = damage;
    }
}
