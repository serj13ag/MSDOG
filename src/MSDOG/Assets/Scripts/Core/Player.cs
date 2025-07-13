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

        private InputService _inputService;
        private UpdateService _updateService;
        private ArenaService _arenaService;

        private HealthBlock _healthBlock;
        private PlayerDamageBlock _playerDamageBlock;

        private List<IAbility> _abilities;

        public int CurrentHealth => _healthBlock.CurrentHealth;
        public int MaxHealth => _healthBlock.MaxHealth;

        public event Action OnHealthChanged
        {
            add => _healthBlock.OnHealthChanged += value;
            remove => _healthBlock.OnHealthChanged -= value;
        }

        public void Init(InputService inputService, UpdateService updateService, ArenaService arenaService,
            AbilityFactory abilityFactory)
        {
            _arenaService = arenaService;
            _updateService = updateService;
            _inputService = inputService;

            _healthBlock = new HealthBlock(100);
            _playerDamageBlock = new PlayerDamageBlock(_healthBlock);

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
            HandleMove(deltaTime);
            HandleAbilities(deltaTime);

            _playerDamageBlock.OnUpdate(deltaTime);
        }

        public void RegisterDamager(Guid id, int damage)
        {
            _playerDamageBlock.RegisterDamager(id, damage);
        }

        public void RemoveDamager(Guid id)
        {
            _playerDamageBlock.RemoveDamager(id);
        }

        private void HandleMove(float deltaTime)
        {
            var moveInput = _inputService.MoveInput;
            if (moveInput == Vector2.zero)
            {
                return;
            }

            var moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
            var move = moveDirection * (_moveSpeed * deltaTime);

            var nextPosition = transform.position + move;
            if (Mathf.Abs(nextPosition.x) > _arenaService.HalfSize.X)
            {
                move.x = 0f;
            }

            if (Mathf.Abs(nextPosition.z) > _arenaService.HalfSize.Y)
            {
                move.z = 0f;
            }

            _characterController.Move(move);
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