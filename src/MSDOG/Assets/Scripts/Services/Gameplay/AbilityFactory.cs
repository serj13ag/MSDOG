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
                AbilityType.RoundAttack => new RoundAttackAbility(abilityData, player),
                AbilityType.GunShot => new GunShotAbility(abilityData, player, _projectileFactory),
                AbilityType.BulletHell => new BulletHellAbility(abilityData, player, _projectileFactory),
                AbilityType.BuzzSaw => new BuzzSawAbility(abilityData, player, _projectileFactory),
                AbilityType.PuncturedTank => new PuncturedTankAbility(abilityData, player, _projectileFactory),
                AbilityType.EnergyLine => new EnergyLineAbility(abilityData, player, _projectileFactory),

                AbilityType.AntiGravity => new AntiGravityAbility(abilityData, player),
                AbilityType.EnergyShield => new EnergyShieldAbility(abilityData, player),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}