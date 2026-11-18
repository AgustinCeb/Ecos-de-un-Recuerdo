using Unity.Netcode;
using UnityEngine;

public class RangedAttack : MonoBehaviour, IEnemyAttack
{
    [SerializeField] private int damageVal;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float bulletSpeed;

    private Vector3 target;

    public void EnemyAttack()
    {
        NetworkManager networkManager = NetworkManager.Singleton;

        if (networkManager == null || !networkManager.IsServer) return;

        GameObject bullet = Instantiate(
            bulletPrefab,
            shootPoint.position,
            Quaternion.identity
        );

        BasicBullet basicBullet = bullet.GetComponent<BasicBullet>();
        EnemiesDamage enemiesDamage = bullet.GetComponent<EnemiesDamage>();
        NetworkObject networkObject = bullet.GetComponent<NetworkObject>();

        if (basicBullet == null || enemiesDamage == null || networkObject == null)
        {
            Debug.LogError("La bala no tiene todos los componentes necesarios.");
            Destroy(bullet);
            return;
        }

        basicBullet.SetInitVal(target, bulletSpeed);
        enemiesDamage.setDamage(damageVal);

        networkObject.Spawn();
    }

    public void setTarget(Vector3 tar)
    {
        target = tar;
    }
}