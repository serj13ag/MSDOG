using System;
using UnityEngine;

namespace Gameplay.Blocks
{
    public class HealthBlock
    {
        private readonly int _maxHealth;
        private readonly bool _ignoreDamage;

        private int _currentHealth;

        public int MaxHealth => _maxHealth;
        public int CurrentHealth => _currentHealth;
        public bool HasZeroHealth => _currentHealth == 0;

        public event Action OnHealthChanged;

        public HealthBlock(int maxHealth, bool ignoreDamage = false)
        {
            _maxHealth = maxHealth;
            _ignoreDamage = ignoreDamage;
            _currentHealth = maxHealth;
        }

        public void ReduceHealth(int damage)
        {
            if (_ignoreDamage)
            {
                return;
            }

            if (damage <= 0)
            {
                return;
            }

            if (_currentHealth == 0)
            {
                return;
            }

            _currentHealth -= damage;
            _currentHealth = Mathf.Max(_currentHealth, 0);

            OnHealthChanged?.Invoke();
        }

        public void Heal(int value)
        {
            if (value <= 0)
            {
                return;
            }

            if (_currentHealth == _maxHealth)
            {
                return;
            }

            _currentHealth += value;
            _currentHealth = Mathf.Min(_currentHealth, _maxHealth);

            OnHealthChanged?.Invoke();
        }
    }
}