using Core.Models.Data;
using Gameplay.Factories;
using Gameplay.Interfaces;
using Gameplay.Projectiles;

namespace Gameplay.Abilities.Core
{
    public class GunShotAbility : BaseCooldownAbility
    {
        private readonly AbilityData _abilityData;
        private readonly IEntityWithAbilities _entityWithAbilities;
        private readonly IProjectileFactory _projectileFactory;

        public GunShotAbility(AbilityData abilityData, IEntityWithAbilities entityWithAbilities,
            IProjectileFactory projectileFactory)
            : base(abilityData)
        {
            _abilityData = abilityData;
            _entityWithAbilities = entityWithAbilities;
            _projectileFactory = projectileFactory;
        }

        protected override void InvokeAction()
        {
            var projectileSpawnData =
                new ProjectileSpawnData(_entityWithAbilities.GetAbilitySpawnPosition(_abilityData.AbilityType),
                    _entityWithAbilities.GetForwardDirection(), _abilityData);
            _projectileFactory.CreateProjectile(projectileSpawnData);
        }
    }
}