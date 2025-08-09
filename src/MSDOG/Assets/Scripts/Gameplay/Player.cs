using System;
using System.Collections.Generic;
using System.Linq;
using Core.Controllers;
using Core.Interfaces;
using Core.Models.Data;
using Core.Services;
using Gameplay.Abilities;
using Gameplay.Factories;
using Gameplay.Services;
using UnityEngine;

namespace Gameplay
{
    public class Player : MonoBehaviour, IUpdatable
    {
        private const int MaxDamageReductionPercent = 80;

        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _moveSpeed = 6f;
        [SerializeField] private float _rotationSpeed = 720f;

        private UpdateController _updateController;
        private AbilityFactory _abilityFactory;

        private HealthBlock _healthBlock;
        private ExperienceBlock _experienceBlock;
        private PlayerDamageBlock _playerDamageBlock;
        private InputMoveBlock _moveBlock;
        private AnimationBlock _animationBlock;

        private readonly Dictionary<Guid, IAbility> _abilities = new Dictionary<Guid, IAbility>();
        private float _additionalMoveSpeed;
        private float _nitroMultiplier = 1f;
        private int _damageReductionPercent;
        private bool _movementIsActive;
        private bool _abilitiesIsActive;

        public CharacterController CharacterController => _characterController;
        public AnimationBlock AnimationBlock => _animationBlock;

        public float RotationSpeed => _rotationSpeed;
        public float CurrentMoveSpeed => (_movementIsActive ? _moveSpeed + _additionalMoveSpeed : 0f) * _nitroMultiplier;
        public bool HasNitro => _nitroMultiplier > 1f;
        public int CurrentDamageReductionPercent => _damageReductionPercent;

        public int CurrentHealth => _healthBlock.CurrentHealth;
        public int MaxHealth => _healthBlock.MaxHealth;
        public bool IsFullHealth => _healthBlock.CurrentHealth == _healthBlock.MaxHealth;

        public int CurrentExperience => _experienceBlock.CurrentExperience;
        public int MaxExperience => _experienceBlock.MaxExperience;

        public event Action OnHealthChanged
        {
            add => _healthBlock.OnHealthChanged += value;
            remove => _healthBlock.OnHealthChanged -= value;
        }

        public event Action OnExperienceChanged
        {
            add => _experienceBlock.OnExperienceChanged += value;
            remove => _experienceBlock.OnExperienceChanged -= value;
        }

        public void Init(InputService inputService, UpdateController updateController, ArenaService arenaService,
            AbilityFactory abilityFactory, IDataService dataService, IProgressService progressService)
        {
            _abilityFactory = abilityFactory;
            _updateController = updateController;

            _movementIsActive = true;

            _animationBlock = new AnimationBlock(_animator);
            _healthBlock = new HealthBlock(100, progressService.EasyModeEnabled);
            _experienceBlock = new ExperienceBlock(dataService);
            _playerDamageBlock = new PlayerDamageBlock(this, _healthBlock);
            _moveBlock = new InputMoveBlock(this, inputService, arenaService);

            updateController.Register(this);
        }

        public void OnUpdate(float deltaTime)
        {
            HandleAbilities(deltaTime);

            _playerDamageBlock.OnUpdate(deltaTime);
            _moveBlock.OnUpdate(deltaTime);
        }

        public void MovementSetActive(bool value)
        {
            _movementIsActive = value;
            _animationBlock.SetMoveless(!value);
        }

        public void RegisterDamager(Guid id, int damage)
        {
            _playerDamageBlock.RegisterDamager(id, damage);
        }

        public void RegisterProjectileDamager(Guid id, int damage)
        {
            _playerDamageBlock.RegisterProjectileDamager(id, damage);
        }

        public void RemoveDamager(Guid id)
        {
            _playerDamageBlock.RemoveDamager(id);
        }

        public void CollectExperience(int experience)
        {
            _experienceBlock.AddExperience(experience);
        }

        public void ResetExperience()
        {
            _experienceBlock.ResetExperience();
        }

        public void AddAbility(Guid id, AbilityData abilityData)
        {
            var ability = _abilityFactory.CreateAbility(abilityData, this);

            if (_abilitiesIsActive)
            {
                ability.Activate();
            }

            _abilities.Add(id, ability);
        }

        public void RemoveAbility(Guid id)
        {
            _abilities[id].Deactivate();
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

        public void ChangeAdditionalSpeed(float speed)
        {
            var newAdditionalSpeed = _additionalMoveSpeed + speed;
            newAdditionalSpeed = Mathf.Max(newAdditionalSpeed, 0);
            _additionalMoveSpeed = newAdditionalSpeed;
        }

        public void ChangeDamageReductionPercent(int damageReductionPercent)
        {
            var newDamageReductionPercent = _damageReductionPercent + damageReductionPercent;
            newDamageReductionPercent = Mathf.Clamp(newDamageReductionPercent, 0, MaxDamageReductionPercent);
            _damageReductionPercent = newDamageReductionPercent;
        }

        public void Heal(int value)
        {
            _healthBlock.Heal(value);
        }

        public void SetNitro(float moveSpeedMultiplier)
        {
            _nitroMultiplier = moveSpeedMultiplier;
        }

        public void ResetNitro()
        {
            _nitroMultiplier = 1f;
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

            return transform.position + offset;
        }

        private void HandleAbilities(float deltaTime)
        {
            foreach (var ability in _abilities.Values.ToArray())
            {
                ability.OnUpdate(deltaTime);
            }
        }

        private void OnDestroy()
        {
            _updateController.Remove(this);
        }
    }
}