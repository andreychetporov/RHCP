using Game.Enemy;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100;

    public ReactiveValue<float> Health { get; private set; }

    private NewEnemyCharacterController _controller;

    private void Awake()
    {
        _controller = GetComponent<NewEnemyCharacterController>();

        Health = new ReactiveValue<float>(_maxHealth);

        Health.OnChanged += OnHealthChanged;
    }

    public void TakeDamage(float damage)
    {
        Health.Value -= damage;

        _controller?.ApplyHitSlow();

        if (Health.Value <= 0)
            Die();
    }

    private void OnHealthChanged(float newHealth)
    {
        Debug.Log($"HP: {newHealth}");
    }

    private void Die()
    {
        Debug.Log("Dead");
    }
}