using System.Collections.Generic;
using Common;
using Core.Models.Data;
using Gameplay.Interfaces;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.Abilities.Core
{
    public class RoundAttackAbility : BaseCooldownAbility
    {
        private readonly IEntityWithAbilities _entityWithAbilities;
        private readonly int _damage;
        private readonly float _radius;
        private readonly Collider[] _hitBuffer = new Collider[32];

        public RoundAttackAbility(AbilityData abilityData, IEntityWithAbilities entityWithAbilities)
            : base(abilityData)
        {
            _entityWithAbilities = entityWithAbilities;
            _damage = abilityData.Damage;
            _radius = abilityData.Size;
        }

        protected override void InvokeAction()
        {
            Slash();
        }

        private void Slash()
        {
            var hitEnemies = DetectEnemiesInRadius();
            foreach (var enemy in hitEnemies)
            {
                enemy.TakeHitDamage(_damage);
            }
        }

        private List<IHitDamageableEntity> DetectEnemiesInRadius()
        {
            var hitEnemies = new List<IHitDamageableEntity>();

            var circleCenter = _entityWithAbilities.GetPosition();
            var hits = Physics.OverlapSphereNonAlloc(circleCenter, _radius, _hitBuffer, Constants.LayerMasks.EnemyLayer);
            for (var i = 0; i < hits; i++)
            {
                var collider = _hitBuffer[i];
                if (collider.gameObject.TryGetComponentInHierarchy<IHitDamageableEntity>(out var enemy))
                {
                    hitEnemies.Add(enemy);
                }
            }

            return hitEnemies;
        }
    }
}