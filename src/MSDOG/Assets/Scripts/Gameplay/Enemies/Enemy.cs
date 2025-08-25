using System;
using Core.Controllers;
using Core.Interfaces;
using Core.Models.Data;
using Core.Services;
using Gameplay.Enemies.EnemyBehaviour;
using Gameplay.Factories;
using Gameplay.Projectiles;
using Gameplay.Providers;
using UnityEngine;
using UnityEngine.AI;
using Utility;
using Utility.Pools;
using VContainer;

namespace Gameplay.Enemies
{
    public class Enemy : BasePooledObject, IUpdatable
    {
        private readonly Vector3 _enemyProjectileOffset = Vector3.up * 0.8f;
        private readonly Vector3 _damageTextOffset = Vector3.up * 3f;

        [SerializeField] private Transform _modelRootTransform;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Animator _animator;
        [SerializeField] private ColliderEventProvider _damagePlayerColliderTriggerEnterProvider;
        [SerializeField] private AnimatorEventsProvider _animatorEventsProvider;

        private IUpdateController _updateController;
        private IExperiencePieceFactory _experiencePieceFactory;
        private IProjectileFactory _projectileFactory;
        private IVfxFactory _vfxFactory;
        private IDataService _dataService;
        private IPlayerProvider _playerProvider;
        private IDamageTextFactory _damageTextFactory;

        private Guid _id;
        private int _damage;
        private float _cooldown;
        private float _projectileSpeed;
        private int _experience;
        private HealthBlock _healthBlock;
        private AnimationBlock _animationBlock;
        private IEnemyStateMachine _stateMachine;

        private bool _waitingToShootProjectile;

        public Guid Id => _id;
        public NavMeshAgent Agent => _agent;
        public AnimationBlock AnimationBlock => _animationBlock;
        public Vector3 ModelRootPosition => _modelRootTransform.position;
        public Player Player => _playerProvider.Player;
        public int Damage => _damage;
        public float Cooldown => _cooldown;

        public EnemyDeathkit DeathkitPrefab { get; private set; }

        public int CurrentHealth => _healthBlock.CurrentHealth;
        public int MaxHealth => _healthBlock.MaxHealth;

        public event Action<Enemy> OnDied;
        public event EventHandler<EventArgs> OnHealthChanged;

        [Inject]
        public void Construct(IUpdateController updateController, IExperiencePieceFactory experiencePieceFactory,
            IProjectileFactory projectileFactory, IDataService dataService, IVfxFactory vfxFactory,
            IPlayerProvider playerProvider, IDamageTextFactory damageTextFactory)
        {
            _damageTextFactory = damageTextFactory;
            _playerProvider = playerProvider;
            _updateController = updateController;
            _experiencePieceFactory = experiencePieceFactory;
            _projectileFactory = projectileFactory;
            _dataService = dataService;
            _vfxFactory = vfxFactory;
        }

        public void Init(EnemyData data, Vector3 position)
        {
            _id = Guid.NewGuid();
            _damage = data.Damage;
            _cooldown = data.Cooldown;
            _projectileSpeed = data.ProjectileSpeed;
            _experience = data.Experience;
            DeathkitPrefab = data.DeathkitPrefab;

            _healthBlock = new HealthBlock(data.MaxHealth);
            _animationBlock = new AnimationBlock(_animator);

            _stateMachine = data.Type switch
            {
                EnemyType.Wanderer => new WandererBehaviourStateMachine(this, _damagePlayerColliderTriggerEnterProvider),
                EnemyType.Melee => new MeleeBehaviourStateMachine(this, _damagePlayerColliderTriggerEnterProvider),
                EnemyType.Range => new RangeBehaviourStateMachine(this, _damagePlayerColliderTriggerEnterProvider),
                _ => throw new ArgumentOutOfRangeException(nameof(data.Type), data.Type, null),
            };

            _agent.speed = data.Speed;

            transform.position = position;

            _healthBlock.OnHealthChanged += OnHealthBlockHealthChanged;

            if (_animatorEventsProvider)
            {
                _animatorEventsProvider.OnAnimationAttackHit += OnAnimationAttackHit;
            }

            _updateController.Register(this);
        }

        public void OnUpdate(float deltaTime)
        {
            _stateMachine.OnUpdate(deltaTime);
        }

        private void OnAnimationAttackHit()
        {
            if (!_waitingToShootProjectile)
            {
                return;
            }

            ShootProjectileTowardsPlayer();
            _waitingToShootProjectile = false;
        }

        public void Shoot()
        {
            _waitingToShootProjectile = true;
            _animationBlock.TriggerAttack();
        }

        public void Kill()
        {
            TakeDamage(_healthBlock.CurrentHealth);
        }

        public void TakeDamage(int damage)
        {
            var healthBefore = _healthBlock.CurrentHealth;
            _healthBlock.ReduceHealth(damage);
            var healthAfter = _healthBlock.CurrentHealth;
            var damageDealt = healthBefore - healthAfter;
            _damageTextFactory.CreateDamageTextEffect(damageDealt, transform.position + _damageTextOffset);

            if (_healthBlock.HasZeroHealth)
            {
                _experiencePieceFactory.CreateExperiencePiece(transform.position, _experience);

                Release();

                OnDied?.Invoke(this);
            }
        }

        private void OnHealthBlockHealthChanged()
        {
            OnHealthChanged?.Invoke(this, EventArgs.Empty);
        }

        private void ShootProjectileTowardsPlayer()
        {
            var directionToPlayer = (_playerProvider.Player.transform.position - transform.position).normalized;
            directionToPlayer.y = 0f;

            var spawnPosition = transform.position + _enemyProjectileOffset;
            var projectileSpawnData = new ProjectileSpawnData(spawnPosition, directionToPlayer, Damage,
                _projectileSpeed, 0, 0f, 0f, 0f, _dataService.GetEnemyProjectileData());
            _projectileFactory.CreateEnemyProjectile(projectileSpawnData);
        }

        protected override void Cleanup()
        {
            base.Cleanup();

            _stateMachine.Dispose();

            _updateController.Remove(this);

            _healthBlock.OnHealthChanged -= OnHealthBlockHealthChanged;

            if (_animatorEventsProvider)
            {
                _animatorEventsProvider.OnAnimationAttackHit -= OnAnimationAttackHit;
            }
        }
    }
}