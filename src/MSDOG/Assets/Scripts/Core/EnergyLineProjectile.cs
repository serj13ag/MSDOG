using System.Collections.Generic;
using Constants;
using Core.Enemies;
using DTO;
using Helpers;
using Interfaces;
using Services;
using UnityEngine;
using VContainer;

namespace Core
{
    public class EnergyLineProjectile : MonoBehaviour, IUpdatable
    {
        private readonly Vector3 _playerProjectileOffset = Vector3.up * 1f; // TODO: to player?
        private const float LaserRange = 15f;

        [SerializeField] private GameObject _boxObject;
        [SerializeField] private GameObject _spriteObject;

        private UpdateService _updateService;

        private readonly Collider[] _hitBuffer = new Collider[32];

        private Vector3 _forwardDirection;
        private int _damage;
        private float _size;

        private float _timeTillDamage;
        private float _tickTimeout;
        private float _timeTillDestroy;
        private Vector3 _direction;
        private Player _player;

        [Inject]
        public void Construct(UpdateService updateService)
        {
            _updateService = updateService;
            updateService.Register(this);
        }

        public void Init(CreateProjectileDto createProjectileDto)
        {
            _player = createProjectileDto.Player;
            _damage = createProjectileDto.AbilityData.Damage;
            _size = createProjectileDto.AbilityData.Size;
            _direction = createProjectileDto.ForwardDirection;
            _tickTimeout = createProjectileDto.AbilityData.TickTimeout;
            _timeTillDamage = _tickTimeout;
            _timeTillDestroy = createProjectileDto.AbilityData.Lifetime;

            transform.rotation = Quaternion.LookRotation(_direction);
            transform.position = _player.transform.position + _playerProjectileOffset + _direction * (LaserRange / 2f);
            _boxObject.transform.localScale = new Vector3(_size, 0.5f, LaserRange);

            var t = Mathf.InverseLerp(0.3f, 1.6f, _size);
            var scale = Mathf.LerpUnclamped(0.2f, 1.2f, t);
            _spriteObject.transform.localScale = new Vector3(scale, 1f, 1f);
        }

        public void OnUpdate(float deltaTime)
        {
            transform.position = _player.transform.position + _playerProjectileOffset + _direction * (LaserRange / 2f);

            _timeTillDestroy -= deltaTime;
            if (_timeTillDestroy < 0f)
            {
                Destroy(gameObject);
                return;
            }

            if (_timeTillDamage > 0f)
            {
                _timeTillDamage -= deltaTime;
                return;
            }

            Damage();
            _timeTillDamage = _tickTimeout;
        }

        private void Damage()
        {
            var hitEnemies = DetectEnemiesInLaserBox();
            foreach (var enemy in hitEnemies)
            {
                enemy.TakeDamage(_damage);
            }
        }

        private List<Enemy> DetectEnemiesInLaserBox()
        {
            var hitEnemies = new List<Enemy>();

            var currentStartPos = _player.transform.position;
            var boxCenter = currentStartPos + _direction * (LaserRange / 2f);
            var boxSize = new Vector3(_size, _size, LaserRange);
            var boxRotation = Quaternion.LookRotation(_direction);

            var hits = Physics.OverlapBoxNonAlloc(boxCenter, boxSize / 2f, _hitBuffer, boxRotation,
                Settings.LayerMasks.EnemyLayer);
            for (var i = 0; i < hits; i++)
            {
                var hitCollider = _hitBuffer[i];
                if (hitCollider.gameObject.TryGetComponentInHierarchy<Enemy>(out var enemy))
                {
                    hitEnemies.Add(enemy);
                }
            }

            return hitEnemies;
        }

        private void OnDestroy()
        {
            _updateService.Remove(this);
        }
    }
}