using System;
using Core;
using Core.Abilities;
using Data;

namespace Services.Gameplay
{
    public class AbilityFactory
    {
        private readonly ProjectileFactory _projectileFactory;

        public AbilityFactory(ProjectileFactory projectileFactory)
        {
            _projectileFactory = projectileFactory;
        }

        public IAbility CreateAbility(AbilityData abilityData, Player player)
        {
            return abilityData.AbilityType switch
            {
                AbilityType.CuttingBlow => new CuttingBlowAbility(abilityData, player),
                AbilityType.GunShot => new GunShotAbility(abilityData, player, _projectileFactory),
                AbilityType.BulletHell => new BulletHellAbility(abilityData, player, _projectileFactory),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}