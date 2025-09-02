using System;
using System.Collections.Generic;
using Gameplay.Abilities.Core;
using Gameplay.Interfaces;
using UnityEngine;

namespace Gameplay.Blocks
{
    public class AbilityBlock
    {
        private readonly IEntityWithPosition _entityWithPosition;

        private readonly Dictionary<Guid, IAbility> _abilities = new Dictionary<Guid, IAbility>();
        private bool _abilitiesIsActive;

        public AbilityBlock(IEntityWithPosition entityWithPosition)
        {
            _entityWithPosition = entityWithPosition;
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var ability in _abilities)
            {
                ability.Value.OnUpdate(deltaTime);
            }
        }

        public void AddAbility(Guid id, IAbility ability)
        {
            if (_abilitiesIsActive)
            {
                ability.Activate();
            }

            _abilities.Add(id, ability);
        }

        public void RemoveAbility(Guid id)
        {
            _abilities[id].Deactivate();
            _abilities[id].Dispose();
            _abilities.Remove(id);
        }

        public void AbilitiesSetActive(bool isActive)
        {
            if (isActive != _abilitiesIsActive)
            {
                foreach (var ability in _abilities)
                {
                    if (isActive)
                    {
                        ability.Value.Activate();
                    }
                    else
                    {
                        ability.Value.Deactivate();
                    }
                }

                _abilitiesIsActive = isActive;
            }
        }

        public Vector3 GetAbilitySpawnPosition(AbilityType abilityType)
        {
            Vector3 offset;
            switch (abilityType)
            {
                case AbilityType.CuttingBlow:
                case AbilityType.RoundAttack:
                    offset = Vector3.up * 1.55f;
                    break;
                case AbilityType.GunShot:
                case AbilityType.BulletHell:
                case AbilityType.BuzzSaw:
                case AbilityType.EnergyLine:
                    offset = Vector3.up * 1f;
                    break;
                case AbilityType.PuncturedTank:
                case AbilityType.AntiGravity:
                case AbilityType.EnergyShield:
                    offset = Vector3.zero;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(abilityType), abilityType, null);
            }

            return _entityWithPosition.GetPosition() + offset;
        }
    }
}