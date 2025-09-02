using System;
using Core.Models.Data;
using Core.Services;
using Gameplay.Abilities.Core;
using Gameplay.Blocks;
using Gameplay.Controllers;
using Gameplay.Factories;
using Gameplay.Interfaces;
using Gameplay.Services;
using UnityEngine;
using VContainer;

namespace Gameplay
{
    public class Player : MonoBehaviour, IUpdatable
    {
        private const int MaxDamageReductionPercent = 80;

        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _moveSpeed = 6f;
        [SerializeField] private float _rotationSpeed = 720f;

        private IGameplayUpdateController _updateController;
        private IAbilityFactory _abilityFactory;
        private IInputService _inputService;
        private IDataService _dataService;
        private IArenaService _arenaService;
        private IProgressService _progressService;

        private HealthBlock _healthBlock;
        private ExperienceBlock _experienceBlock;
        private PlayerDamageBlock _playerDamageBlock;
        private InputMoveBlock _moveBlock;
        private AnimationBlock _animationBlock;
        private AbilityBlock _abilityBlock;
        private PlayerSpeedBlock _playerSpeedBlock;

        private int _damageReductionPercent;

        public float RotationSpeed => _rotationSpeed;
        public float BaseMoveSpeed => _moveSpeed;
        public float CurrentMoveSpeed => _playerSpeedBlock.GetCurrentMoveSpeed();
        public bool HasNitro => _playerSpeedBlock.HasNitro;

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

        [Inject]
        public void Construct(IInputService inputService, IGameplayUpdateController updateController, IArenaService arenaService,
            IAbilityFactory abilityFactory, IDataService dataService, IProgressService progressService)
        {
            _progressService = progressService;
            _arenaService = arenaService;
            _dataService = dataService;
            _inputService = inputService;
            _abilityFactory = abilityFactory;
            _updateController = updateController;
        }

        public void Init()
        {
            _animationBlock = new AnimationBlock(_animator);
            _healthBlock = new HealthBlock(100, _progressService.EasyModeEnabled);
            _experienceBlock = new ExperienceBlock(_dataService);
            _playerDamageBlock = new PlayerDamageBlock(this, _healthBlock);
            _moveBlock = new InputMoveBlock(this, _characterController, _inputService, _arenaService);
            _abilityBlock = new AbilityBlock(this);
            _playerSpeedBlock = new PlayerSpeedBlock(this);

            _playerSpeedBlock.SetActive(true);

            _updateController.Register(this);
        }

        public void OnUpdate(float deltaTime)
        {
            _playerDamageBlock.OnUpdate(deltaTime);
            _moveBlock.OnUpdate(deltaTime);
            _abilityBlock.OnUpdate(deltaTime);
            _animationBlock.SetRunning(_moveBlock.IsMoving);
        }

        public void MovementSetActive(bool value)
        {
            _playerSpeedBlock.SetActive(value);
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
            _abilityBlock.AddAbility(id, ability);
        }

        public void RemoveAbility(Guid id)
        {
            _abilityBlock.RemoveAbility(id);
        }

        public void AbilitiesSetActive(bool isActive)
        {
            _abilityBlock.AbilitiesSetActive(isActive);
        }

        public void ChangeAdditionalSpeed(float speed)
        {
            _playerSpeedBlock.ChangeAdditionalSpeed(speed);
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

        public void SetNitro(float nitroMultiplier)
        {
            _playerSpeedBlock.SetNitro(nitroMultiplier);
        }

        public void ResetNitro()
        {
            _playerSpeedBlock.ResetNitro();
        }

        public Vector3 GetAbilitySpawnPosition(AbilityType abilityType)
        {
            return _abilityBlock.GetAbilitySpawnPosition(abilityType);
        }

        private void OnDestroy()
        {
            _updateController.Remove(this);
        }
    }
}