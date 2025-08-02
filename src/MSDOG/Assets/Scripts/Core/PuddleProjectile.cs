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
    public class PuddleProjectile : MonoBehaviour, IUpdatable
    {
        private const float TimeToScale = 0.5f;

        private UpdateService _updateService;

        private readonly Collider[] _hitBuffer = new Collider[32];

        private Vector3 _forwardDirection;
        private int _damage;
        private float _size;

        private float _timeTillDamage;
        private float _tickTimeout;
        private float _timeTillDestroy;
        private float _scaleTime;

        [Inject]
        public void Construct(UpdateService updateService)
        {
            _updateService = updateService;
            updateService.Register(this);
        }

        public void Init(CreateProjectileDto createProjectileDto)
        {
            _damage = createProjectileDto.AbilityData.Damage;
            _size = createProjectileDto.AbilityData.Size;
            _tickTimeout = createProjectileDto.AbilityData.TickTimeout;
            _timeTillDamage = _tickTimeout;
            _timeTillDestroy = createProjectileDto.AbilityData.Lifetime;

            SetLocalScale(0f);
        }

        public void OnUpdate(float deltaTime)
        {
            _scaleTime += deltaTime;
            if (_scaleTime < TimeToScale)
            {
                var t = _scaleTime / TimeToScale;
                var size = Mathf.Lerp(0f, _size, t);
                SetLocalScale(size);
            }
            else
            {
                SetLocalScale(_size);
            }

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
            var hitEnemies = DetectEnemiesInSphere();
            foreach (var enemy in hitEnemies)
            {
                enemy.TakeDamage(_damage);
            }
        }

        private List<Enemy> DetectEnemiesInSphere()
        {
            var hitEnemies = new List<Enemy>();

            var hits = Physics.OverlapSphereNonAlloc(transform.position, _size / 2f, _hitBuffer, Settings.LayerMasks.EnemyLayer);
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

        private void SetLocalScale(float scale)
        {
            transform.localScale = new Vector3(scale, 0.5f, scale);
        }

        private void OnDestroy()
        {
            _updateService.Remove(this);
        }
    }
}