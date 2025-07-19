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
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _moveSpeed = 6f;

        private UpdateService _updateService;
        private AbilityFactory _abilityFactory;

        private HealthBlock _healthBlock;
        private ExperienceBlock _experienceBlock;
        private PlayerDamageBlock _playerDamageBlock;
        private InputMoveBlock _moveBlock;

        private readonly Dictionary<Guid, IAbility> _abilities = new Dictionary<Guid, IAbility>();
        private float _additionalMoveSpeed;

        public CharacterController CharacterController => _characterController;
        public float MoveSpeed => _moveSpeed + _additionalMoveSpeed;

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
            AbilityFactory abilityFactory, DataService dataService)
        {
            _abilityFactory = abilityFactory;
            _updateService = updateService;

            _healthBlock = new HealthBlock(100);
            _experienceBlock = new ExperienceBlock();
            _playerDamageBlock = new PlayerDamageBlock(_healthBlock);
            _moveBlock = new InputMoveBlock(this, inputService, arenaService);

            updateService.Register(this);
        }

        public void OnUpdate(float deltaTime)
        {
            HandleAbilities(deltaTime);

            _playerDamageBlock.OnUpdate(deltaTime);
            _moveBlock.OnUpdate(deltaTime);
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