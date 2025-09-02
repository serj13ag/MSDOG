using System;
using System.Collections.Generic;
using Common;
using Gameplay.Abilities.Core;
using Gameplay.Controllers;
using Gameplay.Enemies;
using Gameplay.Services;
using UnityEngine;
using Utility.Extensions;
using VContainer;

namespace Gameplay.Projectiles.Views
{
    public class EnergyLineProjectileView : BaseProjectileView
    {
        private const float LaserRange = 15f;

        [SerializeField] private GameObject _spriteObject;

        private readonly Collider[] _hitBuffer = new Collider[32];
        private Player _player;

        [Inject]
        public void Construct(IGameplayUpdateController updateController, IArenaService arenaService)
        {
            ConstructBase(updateController, arenaService);
        }

        public void Init(Projectile projectile, Player player)
        {
            InitBase(projectile);

            _player = player;

            UpdateView(projectile.ForwardDirection, projectile.Size);
        }

        protected override void OnUpdated(float deltaTime)
        {
            transform.position = _player.GetAbilitySpawnPosition(AbilityType.EnergyLine) +
                                 Projectile.ForwardDirection * (LaserRange / 2f);
        }

        protected override void OnTickTimeoutRaised(object sender, EventArgs e)
        {
            base.OnTickTimeoutRaised(sender, e);

            DealDamage();
        }

        private void UpdateView(Vector3 forwardDirection, float size)
        {
            transform.rotation = Quaternion.LookRotation(forwardDirection);
            transform.position = _player.GetAbilitySpawnPosition(AbilityType.EnergyLine) + forwardDirection * (LaserRange / 2f);

            var t = Mathf.InverseLerp(0.3f, 1.6f, size);
            var scale = Mathf.LerpUnclamped(0.2f, 1.2f, t);
            _spriteObject.transform.localScale = new Vector3(scale, 1f, 1f);
        }

        private void DealDamage()
        {
            var hitEnemies = DetectEnemiesInLaserBox();
            foreach (var enemy in hitEnemies)
            {
                Projectile.OnHit(enemy);
            }
        }

        private List<Enemy> DetectEnemiesInLaserBox()
        {
            var hitEnemies = new List<Enemy>();

            var currentStartPos = _player.transform.position;
            var boxCenter = currentStartPos + Projectile.ForwardDirection * (LaserRange / 2f);
            var boxSize = new Vector3(Projectile.Size, Projectile.Size, LaserRange);
            var boxRotation = Quaternion.LookRotation(Projectile.ForwardDirection);

            var hits = Physics.OverlapBoxNonAlloc(boxCenter, boxSize / 2f, _hitBuffer, boxRotation,
                Constants.LayerMasks.EnemyLayer);
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
    }
}