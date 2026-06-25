using Unity.Netcode;
using UnityEngine;

public class RangedAttack : MonoBehaviour, IEnemyAttack
{
    [SerializeField] int damageVal;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform shootPoint;
    [SerializeField] float bulletSpeed;
    Vector3 target;
    public void EnemyAttack()
    {
        GameObject bullet = Instantiate(bulletPrefab,shootPoint.position,Quaternion.identity);
        bullet.GetComponent<NetworkObject>().Spawn();
        bullet.GetComponent<EnemiesDamage>().setDamage(damageVal);
        bullet.GetComponent<BasicBullet>().setInitVal(target, bulletSpeed);
    }
    public void setTarget(Vector3 tar)
    {
        target = tar;
    }
}
