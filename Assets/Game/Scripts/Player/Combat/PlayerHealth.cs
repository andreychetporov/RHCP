using System;
using UnityEngine;
using Game;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private float damageCooldown = 0.5f;

    private HealthPointController _health;
    private float _damageTimer;

    public event Action<int, int> HealthChanged;
    public event Action Died;

    public HealthPointController Controller => _health;
    public int CurrentHealth => _health != null ? _health.Health.Value : 0;
    public int MaxHealth => _health != null ? _health.MaxHealth : maxHealth;
    public bool CanTakeDamage => _damageTimer <= 0f;

    private void Awake()
    {
        _health = new HealthPointController(maxHealth);

        _health.OnDeath += OnDeath;
        _health.Health.OnValueChanged += OnHealthChanged;
    }

    private void Start()
    {
        HealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    private void Update()
    {
        if (_damageTimer > 0f)
            _damageTimer -= Time.deltaTime;
    }

    private void OnDestroy()
    {
        if (_health != null)
        {
            _health.OnDeath -= OnDeath;
            _health.Health.OnValueChanged -= OnHealthChanged;
            _health.Dispose();
        }
    }

    private void OnHealthChanged(int oldValue, int newValue)
    {
        Debug.Log($"HP: {newValue}/{_health.MaxHealth}");
        HealthChanged?.Invoke(newValue, MaxHealth);
    }

    private void OnDeath()
    {
        Debug.Log("Player died");
        Died?.Invoke();

        PlayerController controller = GetComponent<PlayerController>();
        if (controller != null)
            controller.enabled = false;
    }

    public void TakeDamage(int damage)
    {
        if (_health == null)
            return;

        if (_damageTimer > 0f)
            return;

        int oldHealth = _health.Health.Value;

        _health.TakeDamage(damage);

        if (_health.Health.Value < oldHealth)
            _damageTimer = damageCooldown;
    }

    public void Heal(int amount)
    {
        if (_health == null)
            return;

        _health.TakeHeal(amount);
    }
}