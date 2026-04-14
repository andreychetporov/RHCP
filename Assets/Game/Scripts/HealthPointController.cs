using System;

namespace Game
{
    public class HealthPointController : IDisposable
    {
        public event Action OnDeath;

        public ReactiveVariableClamped Health { get; private set; }

        public int MaxHealth { get; private set; }
        public bool IsDead { get; private set; }

        public HealthPointController(int maxHealth) : this(maxHealth, maxHealth) { }

        public HealthPointController(int maxHealth, int currentHealth)
        {
            MaxHealth = maxHealth;
            Health = new ReactiveVariableClamped(currentHealth, 0, maxHealth);
            Health.OnValueChanged += Health_OnChanged;
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0 || IsDead)
                return;

            Health.Value -= damage;
        }

        public void TakeHeal(int heal)
        {
            if (heal <= 0 || IsDead)
                return;

            Health.Value += heal;
        }

        private void Health_OnChanged(int oldValue, int newValue)
        {
            if (IsDead)
                return;

            if (newValue <= 0)
            {
                IsDead = true;
                OnDeath?.Invoke();
            }
        }

        public void Dispose()
        {
            if (Health != null)
                Health.OnValueChanged -= Health_OnChanged;

            Health = null;
            MaxHealth = 0;
            IsDead = false;
        }
    }
}