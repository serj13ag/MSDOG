using System;
using System.Collections.Generic;
using Gameplay.Interfaces;
using UnityEngine;

namespace Gameplay.Blocks
{
    public class AccumulativeDamageBlock
    {
        private const float TakeDamageCooldown = 0.2f;

        private readonly IEntityWithDamageReduction _entityWithDamageReduction;
        private readonly HealthBlock _healthBlock;

        private readonly Dictionary<Guid, int> _damageDealers = new Dictionary<Guid, int>(10);
        private readonly Dictionary<Guid, int> _projectileDamageDealers = new Dictionary<Guid, int>(20);
        private float _timeTillTakeDamage;

        public AccumulativeDamageBlock(IEntityWithDamageReduction entityWithDamageReduction, HealthBlock healthBlock)
        {
            _entityWithDamageReduction = entityWithDamageReduction;
            _healthBlock = healthBlock;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_timeTillTakeDamage > 0f)
            {
                _timeTillTakeDamage -= deltaTime;
                return;
            }

            TakeDamage();
            _timeTillTakeDamage = TakeDamageCooldown;
        }

        public void RegisterDamager(Guid id, int damage)
        {
            if (damage <= 0)
            {
                Debug.LogWarning("Damage is less or equal to zero");
                return;
            }

            if (!_damageDealers.TryAdd(id, damage))
            {
                Debug.LogWarning("Damager has already been registered");
            }
        }

        public void RegisterProjectileDamager(Guid id, int damage)
        {
            if (damage <= 0)
            {
                Debug.LogWarning("Damage is less or equal to zero");
                return;
            }

            if (!_projectileDamageDealers.TryAdd(id, damage))
            {
                Debug.LogWarning("Projectile damager has already been registered");
            }
        }

        public void RemoveDamager(Guid id)
        {
            _damageDealers.Remove(id);
        }

        private void TakeDamage()
        {
            var accumulatedDamage = 0;
            foreach (var damageDealer in _damageDealers)
            {
                accumulatedDamage += damageDealer.Value;
            }

            foreach (var projectileDamageDealer in _projectileDamageDealers)
            {
                accumulatedDamage += projectileDamageDealer.Value;
            }

            var percent = (100 - _entityWithDamageReduction.CurrentDamageReductionPercent) / 100f;
            accumulatedDamage = Mathf.CeilToInt(accumulatedDamage * percent);

            _projectileDamageDealers.Clear();

            _healthBlock.ReduceHealth(accumulatedDamage);
        }
    }
}