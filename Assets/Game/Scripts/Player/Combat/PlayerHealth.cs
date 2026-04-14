using UnityEngine;
using Game;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;

    private HealthPointController _health;

    public HealthPointController Controller => _health;

    private void Awake()
    {
        _health = new HealthPointController(maxHealth);

        _health.OnDeath += OnDeath;
        _health.Health.OnValueChanged += OnHealthChanged;
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
    }

    private void OnDeath()
    {
        Debug.Log("Player died");

        GetComponent<PlayerController>().enabled = false;
    }

    public void TakeDamage(int damage)
    {
        _health.TakeDamage(damage);
    }

    public void Heal(int amount)
    {
        _health.TakeHeal(amount);
    }
}