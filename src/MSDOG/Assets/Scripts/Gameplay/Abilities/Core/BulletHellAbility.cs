using Core.Models.Data;
using Gameplay.Factories;
using Gameplay.Interfaces;
using Gameplay.Projectiles;
using UnityEngine;

namespace Gameplay.Abilities.Core
{
    public class BulletHellAbility : BaseCooldownAbility
    {
        private readonly AbilityData _abilityData;
        private readonly IEntityWithAbilities _entityWithAbilities;
        private readonly IProjectileFactory _projectileFactory;

        public BulletHellAbility(AbilityData abilityData, IEntityWithAbilities entityWithAbilities,
            IProjectileFactory projectileFactory)
            : base(abilityData)
        {
            _abilityData = abilityData;
            _entityWithAbilities = entityWithAbilities;
            _projectileFactory = projectileFactory;
        }

        protected override void InvokeAction()
        {
            Vector3 randomDirection;
            do
            {
                randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            } while (randomDirection == Vector3.zero);

            randomDirection.Normalize();

            var projectileSpawnData =
                new ProjectileSpawnData(_entityWithAbilities.GetAbilitySpawnPosition(_abilityData.AbilityType), randomDirection,
                    _abilityData);
            _projectileFactory.CreateAbilityProjectile(projectileSpawnData);
        }
    }
}