using System;
using Core.Interfaces;
using Core.Models.Data;
using Core.Services;
using Gameplay.Enemies.EnemyBehaviour;
using Gameplay.Projectiles;
using Gameplay.Services;
using UI;
using UnityEngine;
using UnityEngine.AI;
using UtilityComponents;

namespace Gameplay.Enemies
{
    public class Enemy : MonoBehaviour, IUpdatable
    {
        private readonly Vector3 _enemyProjectileOffset = Vector3.up * 0.8f;
        private readonly Vector3 _damageTextOffset = Vector3.up * 3f;

        [SerializeField] private Transform _modelRootTransform;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Animator _animator;
        [SerializeField] private HealthBarDebugView _healthBarDebugView;
        [SerializeField] private ColliderEventProvider _damagePlayerColliderTriggerEnterProvider;
        [SerializeField] private AnimatorEventsProvider _animatorEventsProvider;

        private UpdateService _updateService;
        private GameFactory _gameFactory;
        private ProjectileFactory _projectileFactory;
        private VfxFactory _vfxFactory;

        private Guid _id;
        private Player _player;
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
        public Player Player => _player;
        public int Damage => _damage;
        public float Cooldown => _cooldown;

        public EnemyDeathkit DeathkitPrefab { get; private set; }

        public event Action<Enemy> OnDied;

        public void Init(UpdateService updateService, GameFactory gameFactory, ProjectileFactory projectileFactory, Player player,
            EnemyData data, VfxFactory vfxFactory, DebugService debugService)
        {
            _vfxFactory = vfxFactory;
            _projectileFactory = projectileFactory;
            _gameFactory = gameFactory;
            _updateService = updateService;

            _id = Guid.NewGuid();
            _player = player;
            _damage = data.Damage;
            _cooldown = data.Cooldown;
            _projectileSpeed = data.ProjectileSpeed;
            _experience = data.Experience;
            DeathkitPrefab = data.DeathkitPrefab;

            _healthBlock = new HealthBlock(data.MaxHealth);
            _animationBlock = new AnimationBlock(_animator);
            _healthBarDebugView.Init(_healthBlock, debugService);

            _stateMachine = data.Type switch
            {
                EnemyType.Wanderer => new WandererBehaviourStateMachine(this, _damagePlayerColliderTriggerEnterProvider),
                EnemyType.Melee => new MeleeBehaviourStateMachine(this, _damagePlayerColliderTriggerEnterProvider),
                EnemyType.Range => new RangeBehaviourStateMachine(this, _damagePlayerColliderTriggerEnterProvider),
                _ => throw new ArgumentOutOfRangeException(nameof(data.Type), data.Type, null),
            };

            _agent.speed = data.Speed;

            updateService.Register(this);

            if (_animatorEventsProvider)
            {
                _animatorEventsProvider.OnAnimationAttackHit += OnAnimationAttackHit;
            }
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
            _vfxFactory.CreateDamageTextEffect(damageDealt, transform.position + _damageTextOffset);

            if (_healthBlock.HasZeroHealth)
            {
                _gameFactory.CreateExperiencePiece(transform.position, _experience);
                _vfxFactory.CreateBloodEffect(transform.position + _enemyProjectileOffset);

                OnDied?.Invoke(this);
            }
        }

        private void ShootProjectileTowardsPlayer()
        {
            var directionToPlayer = (_player.transform.position - transform.position).normalized;
            directionToPlayer.y = 0f;

            var spawnPosition = transform.position + _enemyProjectileOffset;
            var projectileSpawnData = new ProjectileSpawnData(spawnPosition, directionToPlayer, _player, Damage,
                _projectileSpeed, 0, 0f, 0f, 0f, null);
            _projectileFactory.CreateEnemyProjectile(projectileSpawnData);
        }

        private void OnDestroy()
        {
            _stateMachine.Dispose();

            _updateService.Remove(this);

            if (_animatorEventsProvider)
            {
                _animatorEventsProvider.OnAnimationAttackHit -= OnAnimationAttackHit;
            }
        }
    }
}