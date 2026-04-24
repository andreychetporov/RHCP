using UnityEngine;
using Game.Enemy;

[RequireComponent(typeof(Collider))]
public class WeaponProjectile : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float lifeTime = 5f;

    private Vector3 _direction;
    private float _speed;
    private int _damage;
    private bool _initialized;

    public void Initialize(Vector3 targetPoint, float speed, int damage)
    {
        Vector3 dir = targetPoint - transform.position;
        dir.z = 0f;

        _direction = dir.normalized;
        _speed = speed;
        _damage = damage;
        _initialized = true;

        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (!_initialized)
            return;

        transform.position += _direction * _speed * Time.deltaTime;

        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        if (other.CompareTag("Player"))
            return;

        EnemyController enemy = other.GetComponentInParent<EnemyController>();

        if (enemy != null)
        {
            enemy.HealthController.TakeDamage(_damage);
        }

        Destroy(gameObject);
    }
}