using Game;
using Game.Audio;
using Game.Blood;
using Game.Enemy;
using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public event Action<int, int> HealthChanged;
    public event Action Died;

    [Header("Settings")]
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private float damageCooldown = 0.5f;

    [Header("Effects")]
    [SerializeField] private SoundData _takeDamageSFX;
    
    public HealthPointController Controller => _health;
    public int CurrentHealth => _health != null ? _health.Health.Value : 0;
    public int MaxHealth => _health != null ? _health.MaxHealth : maxHealth;
    public bool CanTakeDamage => _damageTimer <= 0f;

    private HealthPointController _health;
    private float _damageTimer;

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
        {
            _damageTimer -= Time.deltaTime;
        }
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

        if (newValue < oldValue && newValue > 0)
        {
            _damageTimer = damageCooldown;

            SoundManager.Instance.Get().Initialize(_takeDamageSFX).Play();
            BloodManager.Instance.GetForDamage().Initialize(transform.position, Color.red).Play();
            BloodCanvas.Instance.SpawnBloodSpot(Color.red);
        }

        HealthChanged?.Invoke(newValue, MaxHealth);
    }

    private void OnDeath()
    {
        Debug.Log("Player died");

        SoundManager.Instance.Get().Initialize(_takeDamageSFX).Play();
        BloodManager.Instance.GetForDeath().Initialize(transform.position, Color.red).Play();

        Died?.Invoke();
    }

    public void TakeDamage(int damage)
    {
        if (_damageTimer > 0.0f) { return; }

        _health?.TakeDamage(damage);
    }

    public void Heal(int amount) => _health?.TakeHeal(amount);
}