using System;
using Core.Models.Data;
using Core.Services;
using Gameplay.Blocks;
using Gameplay.Controllers;
using Gameplay.Enemies.EnemyBehaviour;
using Gameplay.Factories;
using Gameplay.Interfaces;
using Gameplay.Projectiles;
using Gameplay.Providers;
using UnityEngine;
using UnityEngine.AI;
using Utility;
using Utility.Pools;
using VContainer;

namespace Gameplay.Enemies
{
    public class Enemy : BasePooledObject, IEnemy, IUpdatable
    {
        private readonly Vector3 _enemyProjectileOffset = Vector3.up * 0.8f;
        private readonly Vector3 _damageTextOffset = Vector3.up * 3f;

        [SerializeField] private Transform _modelRootTransform;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Animator _animator;
        [SerializeField] private ColliderEventProvider _damagePlayerColliderTriggerEnterProvider;
        [SerializeField] private AnimatorEventsProvider _animatorEventsProvider;

        private IGameplayUpdateController _updateController;
        private IExperiencePieceFactory _experiencePieceFactory;
        private IProjectileFactory _projectileFactory;
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
        public Vector3 ModelRootPosition => _modelRootTransform.position;
        public int Damage => _damage;
        public float Cooldown => _cooldown;

        public EnemyDeathkit DeathkitPrefab { get; private set; }

        public int CurrentHealth => _healthBlock.CurrentHealth;
        public int MaxHealth => _healthBlock.MaxHealth;

        public event Action<IEnemy> OnDied;
        public event EventHandler<EventArgs> OnHealthChanged;

        [Inject]
        public void Construct(IGameplayUpdateController updateController, IExperiencePieceFactory experiencePieceFactory,
            IProjectileFactory projectileFactory, IDataService dataService, IPlayerProvider playerProvider,
            IDamageTextFactory damageTextFactory)
        {
            _damageTextFactory = damageTextFactory;
            _playerProvider = playerProvider;
            _updateController = updateController;
            _experiencePieceFactory = experiencePieceFactory;
            _projectileFactory = projectileFactory;
            _dataService = dataService;
        }

        public override void OnGet()
        {
            base.OnGet();

            if (_animatorEventsProvider)
            {
                _animatorEventsProvider.OnAnimationAttackHit += OnAnimationAttackHit;
            }

            _updateController.Register(this);
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

            var enemyBehaviourStateMachineContext = new EnemyBehaviourStateMachineContext(this, _agent, _animationBlock,
                _damagePlayerColliderTriggerEnterProvider, _playerProvider);
            _stateMachine = data.Type switch
            {
                EnemyType.Wanderer => new WandererBehaviourStateMachine(enemyBehaviourStateMachineContext),
                EnemyType.Melee => new MeleeBehaviourStateMachine(enemyBehaviourStateMachineContext),
                EnemyType.Range => new RangeBehaviourStateMachine(enemyBehaviourStateMachineContext),
                _ => throw new ArgumentOutOfRangeException(nameof(data.Type), data.Type, null),
            };

            _agent.speed = data.Speed;

            transform.position = position;

            _healthBlock.OnHealthChanged += OnHealthBlockHealthChanged;
        }

        public void OnUpdate(float deltaTime)
        {
            _stateMachine.OnUpdate(deltaTime);
        }

        public void Shoot()
        {
            _waitingToShootProjectile = true;
            _animationBlock.TriggerAttack();
        }

        public void Kill()
        {
            TakeDamageInner(_healthBlock.CurrentHealth);
        }

        public void TakeProjectileDamage(Guid projectileId, int damage)
        {
            TakeDamageInner(_damage);
        }

        public void TakeHitDamage(int damage)
        {
            TakeDamageInner(damage);
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public Vector3 GetForwardDirection()
        {
            return transform.forward;
        }

        public Quaternion GetRotation()
        {
            return transform.rotation;
        }

        public void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        private void TakeDamageInner(int damage)
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

        private void OnAnimationAttackHit()
        {
            if (!_waitingToShootProjectile)
            {
                return;
            }

            ShootProjectileTowardsPlayer();
            _waitingToShootProjectile = false;
        }

        private void ShootProjectileTowardsPlayer()
        {
            var directionToPlayer = (_playerProvider.Player.GetPosition() - transform.position).normalized;
            directionToPlayer.y = 0f;

            var spawnPosition = transform.position + _enemyProjectileOffset;
            var projectileSpawnData = new ProjectileSpawnData(spawnPosition, directionToPlayer, Damage,
                _projectileSpeed, 0, 0f, 0f, 0f, _dataService.GetEnemyProjectileData());
            _projectileFactory.CreateProjectile(projectileSpawnData);
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