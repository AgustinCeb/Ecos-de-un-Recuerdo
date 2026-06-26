using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class BasicBullet : MonoBehaviour
{
    Vector3 moveDir;
    float bulletSpeed;
    [SerializeField] float lifeTime;
    private void Awake()
    {

    }
    public void setInitVal(Vector3 dir, float speed)
    {
        bulletSpeed = speed;
        moveDir = dir-transform.position;
        moveDir.y=0;
        transform.rotation = Quaternion.LookRotation(moveDir);
    }
    private void Update()
    {
        transform.position += moveDir * bulletSpeed * Time.deltaTime;
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0) GetComponent<NetworkObject>().Despawn();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) GetComponent<NetworkObject>().Despawn();
    }
}
