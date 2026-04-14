using System;

namespace Game
{
    public class HealthPointController : IDisposable
    {
        public event Action OnDeath;

        public ReactiveVariableClamped Health { get; private set; }

        private int _maxHealth = 0;
        public bool _isDeath = false;

        public HealthPointController(int maxHealth) : this(maxHealth, maxHealth) {}

        public HealthPointController(int maxHealth, int currentHealth)
        {
            _maxHealth = maxHealth;

            if (Health != null) { Health.OnValueChanged -= Health_OnChanged; }

            Health = new ReactiveVariableClamped(currentHealth, 0, maxHealth);
            Health.OnValueChanged += Health_OnChanged;
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0 || _isDeath) { return; }

            Health.Value -= damage;
        }

        public void TakeHeal(int heal)
        {
            if (heal < 0 || _isDeath) { return; }

            Health.Value += heal;
        }

        private void Health_OnChanged(int oldValue, int newValue)
        {
            if (newValue <= 0) { OnDeath?.Invoke(); }
        }

        public void Dispose()
        {
            if (Health != null) { Health.OnValueChanged -= Health_OnChanged; }

            Health = null;
            _maxHealth = default;
        }
    }
}