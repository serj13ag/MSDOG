using System;
using Core.Enemies.EnemyBehaviour;
using Data;
using DTO;
using Interfaces;
using Services;
using Services.Gameplay;
using UI;
using UnityEngine;
using UnityEngine.AI;
using UtilityComponents;

namespace Core.Enemies
{
    public class Enemy : MonoBehaviour, IUpdatable
    {
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

        public EnemyType Type { get; private set; }

        public event Action<Enemy> OnDied;

        public void Init(UpdateService updateService, GameFactory gameFactory, ProjectileFactory projectileFactory, Player player,
            EnemyData data, VfxFactory vfxFactory)
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
            Type = data.Type;

            _healthBlock = new HealthBlock(data.MaxHealth);
            _animationBlock = new AnimationBlock(_animator);
            _healthBarDebugView.Init(_healthBlock);

            _stateMachine = data.Type switch
            {
                EnemyType.Wanderer => new WandererBehaviourStateMachine(this),
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
            _healthBlock.ReduceHealth(damage);
            if (_healthBlock.HasZeroHealth)
            {
                _gameFactory.CreateExperiencePiece(transform.position, _experience);
                _vfxFactory.CreateBloodEffect(transform.position);

                OnDied?.Invoke(this);
            }
        }

        private void ShootProjectileTowardsPlayer()
        {
            var directionToPlayer = (_player.transform.position - transform.position).normalized;
            directionToPlayer.y = 0f;
            var createProjectileDto =
                new CreateEnemyProjectileDto(transform.position, directionToPlayer, _player, Damage, _projectileSpeed, 0);
            _projectileFactory.CreateEnemyProjectile(createProjectileDto);
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