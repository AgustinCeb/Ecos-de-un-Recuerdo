using Unity.Netcode;
using UnityEngine;

public class BasicBullet : NetworkBehaviour
{
    private Vector3 _moveDir;
    private float _bulletSpeed;

    [SerializeField] private float _lifeTime = 5f;

    private float _timer;
    private bool _initialized;

    public void SetInitVal(Vector3 dir, float speed)
    {
        _bulletSpeed = speed;

        _moveDir = dir - transform.position;
        _moveDir.y = 0f;

        if (_moveDir.sqrMagnitude > 0.001f)
        {
            _moveDir.Normalize();
            transform.rotation = Quaternion.LookRotation(_moveDir);
        }

        _timer = _lifeTime;
        _initialized = true;
    }

    private void Update()
    {
        if (!IsServer || !IsSpawned || !_initialized) return;

        transform.position += _moveDir * _bulletSpeed * Time.deltaTime;

        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            DespawnBullet();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer || !IsSpawned) return;

        if (other.CompareTag("Player"))
        {
            DespawnBullet();
        }
    }

    private void DespawnBullet()
    {
        if (!IsServer || !IsSpawned) return;

        NetworkObject.Despawn();
    }
}