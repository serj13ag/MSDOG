using System.Collections.Generic;
using Constants;
using Core.Enemies;
using DTO;
using Interfaces;
using Services;
using UnityEngine;

namespace Core
{
    public class EnergyLineProjectile : MonoBehaviour, IUpdatable
    {
        private const float LaserRange = 10f;

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

        public void Init(CreateProjectileDto createProjectileDto, UpdateService updateService)
        {
            _updateService = updateService;

            _player = createProjectileDto.Player;
            _damage = createProjectileDto.AbilityData.Damage;
            _size = createProjectileDto.AbilityData.Size;
            _direction = createProjectileDto.ForwardDirection;
            _tickTimeout = createProjectileDto.AbilityData.TickTimeout;
            _timeTillDamage = _tickTimeout;
            _timeTillDestroy = createProjectileDto.AbilityData.Lifetime;

            transform.localScale = new Vector3(_size, 0.5f, LaserRange);
            transform.rotation = Quaternion.LookRotation(_direction);
            transform.position = _player.transform.position + _direction * (LaserRange / 2f);

            updateService.Register(this);
        }

        public void OnUpdate(float deltaTime)
        {
            transform.position = _player.transform.position + _direction * (LaserRange / 2f);

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

            var hits = Physics.OverlapBoxNonAlloc(boxCenter, boxSize / 2f, _hitBuffer, boxRotation, Settings.LayerMasks.EnemyLayer);
            for (var i = 0; i < hits; i++)
            {
                var hitCollider = _hitBuffer[i];
                if (hitCollider.TryGetComponent<Enemy>(out var enemy))
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