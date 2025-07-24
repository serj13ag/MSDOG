using System;
using System.Collections.Generic;
using System.Linq;
using Core.Abilities;
using Data;
using Interfaces;
using Services;
using Services.Gameplay;
using UnityEngine;

namespace Core
{
    public class Player : MonoBehaviour, IUpdatable
    {
        private const int MaxDamageReductionPercent = 80;

        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _moveSpeed = 6f;
        [SerializeField] private float _rotationSpeed = 720f;

        private UpdateService _updateService;
        private AbilityFactory _abilityFactory;

        private HealthBlock _healthBlock;
        private ExperienceBlock _experienceBlock;
        private PlayerDamageBlock _playerDamageBlock;
        private InputMoveBlock _moveBlock;
        private AnimationBlock _animationBlock;

        private readonly Dictionary<Guid, IAbility> _abilities = new Dictionary<Guid, IAbility>();
        private float _additionalMoveSpeed;
        private int _damageReductionPercent;
        private bool _movementIsActive;

        public CharacterController CharacterController => _characterController;
        public AnimationBlock AnimationBlock => _animationBlock;

        public float RotationSpeed => _rotationSpeed;
        public float CurrentMoveSpeed => _movementIsActive ? _moveSpeed + _additionalMoveSpeed : 0f;
        public int CurrentDamageReductionPercent => _damageReductionPercent;

        public int CurrentHealth => _healthBlock.CurrentHealth;
        public int MaxHealth => _healthBlock.MaxHealth;

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

        public void Init(InputService inputService, UpdateService updateService, ArenaService arenaService,
            AbilityFactory abilityFactory)
        {
            _abilityFactory = abilityFactory;
            _updateService = updateService;

            _movementIsActive = true;

            _animationBlock = new AnimationBlock(_animator);
            _healthBlock = new HealthBlock(100);
            _experienceBlock = new ExperienceBlock();
            _playerDamageBlock = new PlayerDamageBlock(this, _healthBlock);
            _moveBlock = new InputMoveBlock(this, inputService, arenaService);

            updateService.Register(this);
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
            ability.Activate();
            _abilities.Add(id, ability);
        }

        public void RemoveAbility(Guid id)
        {
            _abilities[id].Deactivate();
            _abilities.Remove(id);
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

        private void HandleAbilities(float deltaTime)
        {
            foreach (var ability in _abilities.Values.ToArray())
            {
                ability.OnUpdate(deltaTime);
            }
        }

        private void OnDestroy()
        {
            _updateService.Remove(this);
        }
    }
}