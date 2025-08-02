using System;
using System.Collections.Generic;
using Constants;
using Core.Enemies;
using Helpers;
using Interfaces;
using Services;
using UnityEngine;
using VContainer;

namespace Core.Projectiles
{
    public class EnergyLineProjectileView : MonoBehaviour, IUpdatable
    {
        private readonly Vector3 _playerProjectileOffset = Vector3.up * 1f; // TODO: to player?
        private const float LaserRange = 15f;

        [SerializeField] private GameObject _boxObject;
        [SerializeField] private GameObject _spriteObject;

        private UpdateService _updateService;

        private readonly Collider[] _hitBuffer = new Collider[32];
        private Projectile _projectile;
        private Player _player;

        [Inject]
        public void Construct(UpdateService updateService)
        {
            _updateService = updateService;
            updateService.Register(this);
        }

        public void Init(Projectile projectile, Player player)
        {
            _projectile = projectile;
            _player = player;

            UpdateView(projectile.ForwardDirection, projectile.Size);

            projectile.OnLifetimeEnded += OnProjectileLifetimeEnded;
            projectile.OnTickTimeoutRaised += OnProjectileTickTimeoutRaised;
        }

        public void OnUpdate(float deltaTime)
        {
            _projectile.OnUpdate(deltaTime);

            transform.position = _player.transform.position + _playerProjectileOffset +
                                 _projectile.ForwardDirection * (LaserRange / 2f);
        }

        private void OnProjectileTickTimeoutRaised(object sender, EventArgs e)
        {
            DealDamage();
        }

        private void OnProjectileLifetimeEnded(object sender, EventArgs e)
        {
            Destroy(gameObject);
        }

        private void UpdateView(Vector3 forwardDirection, float size)
        {
            transform.rotation = Quaternion.LookRotation(forwardDirection);
            transform.position = _player.transform.position + _playerProjectileOffset + forwardDirection * (LaserRange / 2f);
            _boxObject.transform.localScale = new Vector3(size, 0.5f, LaserRange);

            var t = Mathf.InverseLerp(0.3f, 1.6f, size);
            var scale = Mathf.LerpUnclamped(0.2f, 1.2f, t);
            _spriteObject.transform.localScale = new Vector3(scale, 1f, 1f);
        }

        private void DealDamage()
        {
            var hitEnemies = DetectEnemiesInLaserBox();
            foreach (var enemy in hitEnemies)
            {
                _projectile.OnHit(enemy);
            }
        }

        private List<Enemy> DetectEnemiesInLaserBox()
        {
            var hitEnemies = new List<Enemy>();

            var currentStartPos = _player.transform.position;
            var boxCenter = currentStartPos + _projectile.ForwardDirection * (LaserRange / 2f);
            var boxSize = new Vector3(_projectile.Size, _projectile.Size, LaserRange);
            var boxRotation = Quaternion.LookRotation(_projectile.ForwardDirection);

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

            _projectile.OnLifetimeEnded += OnProjectileLifetimeEnded;
            _projectile.OnTickTimeoutRaised += OnProjectileTickTimeoutRaised;
        }
    }
}