using System;
using UnityEngine;

namespace Game
{
    public class HealthPointController : IDisposable
    {
        public event Action OnDeath;

        public ReactiveVariableClamped Health { get; private set; }


        public int MaxHealth { get; private set; }
        public bool IsDead => Health.Value <= 0;


        public HealthPointController(int maxHealth) : this(maxHealth, maxHealth) { }

        public HealthPointController(int maxHealth, int currentHealth)
        {
            MaxHealth = maxHealth;

            if (Health != null) { Health.OnValueChanged -= Health_OnChanged; }

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
            if (heal < 0 || IsDead) { return; }

            Health.Value += heal;
        }

        private void Health_OnChanged(int oldValue, int newValue)
        {

            Debug.Log(newValue);
            if (newValue <= 0)
            {
                OnDeath?.Invoke();
            }

        }

        public void Dispose()
        {
            if (Health != null) { Health.OnValueChanged -= Health_OnChanged; }

            Health = null;

            MaxHealth = 0;

        }
    }
}
