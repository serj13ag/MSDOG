using System;
using System.Collections.Generic;
using Core.Abilities;
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

        private HealthBlock _healthBlock;
        private ExperienceBlock _experienceBlock;
        private PlayerDamageBlock _playerDamageBlock;
        private InputMoveBlock _moveBlock;

        private List<IAbility> _abilities;

        public CharacterController CharacterController => _characterController;
        public float MoveSpeed => _moveSpeed;

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
            _updateService = updateService;

            _healthBlock = new HealthBlock(100);
            _experienceBlock = new ExperienceBlock();
            _playerDamageBlock = new PlayerDamageBlock(_healthBlock);
            _moveBlock = new InputMoveBlock(this, inputService, arenaService);

            updateService.Register(this);

            _abilities = new List<IAbility>()
            {
                abilityFactory.CreateCuttingBlowAbility(this),
                abilityFactory.CreateGunShotAbility(this),
                abilityFactory.CreateBulletHellAbility(this),
            }; // TODO: load dynamically
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

        public void RemoveDamager(Guid id)
        {
            _playerDamageBlock.RemoveDamager(id);
        }

        public void CollectExperience(int experience)
        {
            _experienceBlock.AddExperience(experience);
        }

        private void HandleAbilities(float deltaTime)
        {
            foreach (var ability in _abilities)
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