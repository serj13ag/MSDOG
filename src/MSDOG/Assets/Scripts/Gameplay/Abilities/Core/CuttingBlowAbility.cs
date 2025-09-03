using System.Collections.Generic;
using Common;
using Core.Models.Data;
using Gameplay.Interfaces;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.Abilities.Core
{
    public class CuttingBlowAbility : BaseCooldownAbility
    {
        private readonly IEntityWithAbilities _entityWithAbilities;
        private readonly int _damage;
        private readonly float _length;
        private readonly Collider[] _hitBuffer = new Collider[32];

        public CuttingBlowAbility(AbilityData abilityData, IEntityWithAbilities entityWithAbilities)
            : base(abilityData)
        {
            _entityWithAbilities = entityWithAbilities;
            _damage = abilityData.Damage;
            _length = abilityData.Size;
        }

        protected override void InvokeAction()
        {
            Slash();
        }

        private void Slash()
        {
            var enemies = DetectEnemiesInStrikeBox();
            foreach (var enemy in enemies)
            {
                enemy.TakeHitDamage(_damage);
            }
        }

        private List<IHitDamageableEntity> DetectEnemiesInStrikeBox()
        {
            var hitEnemies = new List<IHitDamageableEntity>();

            var boxCenter = _entityWithAbilities.GetPosition();
            var boxSize = new Vector3(_length, Constants.CuttingBlowAbility.BoxHeight, Constants.CuttingBlowAbility.BoxWidth);

            var hits = Physics.OverlapBoxNonAlloc(boxCenter, boxSize * 0.5f, _hitBuffer, Quaternion.identity,
                Constants.LayerMasks.EnemyLayer);
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