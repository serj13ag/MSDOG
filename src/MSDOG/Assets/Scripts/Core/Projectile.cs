using Core.Enemies;
using DTO;
using Interfaces;
using Services;
using UnityEngine;
using UtilityComponents;

namespace Core
{
    public class Projectile : MonoBehaviour, IUpdatable
    {
        [SerializeField] private OnTriggerEnterProvider _onTriggerEnterProvider;

        private UpdateService _updateService;

        private Vector3 _forwardDirection;
        private int _damage;
        private float _speed;
        private int _pierce;

        public void Init(CreateProjectileDto createProjectileDto, UpdateService updateService)
        {
            _updateService = updateService;

            _pierce = createProjectileDto.Pierce;
            _speed = createProjectileDto.Speed;
            _damage = createProjectileDto.Damage;
            _forwardDirection = createProjectileDto.ForwardDirection;

            updateService.Register(this);
            _onTriggerEnterProvider.OnTriggerEntered += OnTriggerEntered;
        }

        public void OnUpdate(float deltaTime)
        {
            transform.position += _forwardDirection.normalized * (_speed * deltaTime);
        }

        private void OnTriggerEntered(Collider other)
        {
            var enemy = other.gameObject.GetComponentInParent<Enemy>();
            if (!enemy)
            {
                return;
            }

            enemy.TakeDamage(_damage);

            if (_pierce > 0)
            {
                _pierce -= 1;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            _updateService.Remove(this);
            _onTriggerEnterProvider.OnTriggerEntered -= OnTriggerEntered;
        }
    }
}