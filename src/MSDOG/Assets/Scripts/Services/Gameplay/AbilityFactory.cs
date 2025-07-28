using System;
using Core;
using Core.Abilities;
using Data;

namespace Services.Gameplay
{
    public class AbilityFactory
    {
        private readonly ProjectileFactory _projectileFactory;
        private readonly VfxFactory _vfxFactory;
        private readonly DataService _dataService;

        public AbilityFactory(ProjectileFactory projectileFactory, VfxFactory vfxFactory, DataService dataService)
        {
            _dataService = dataService;
            _projectileFactory = projectileFactory;
            _vfxFactory = vfxFactory;
        }

        public IAbility CreateAbility(AbilityData abilityData, Player player)
        {
            return abilityData.AbilityType switch
            {
                AbilityType.CuttingBlow => new CuttingBlowAbility(abilityData, player, _vfxFactory, _dataService),
                AbilityType.RoundAttack => new RoundAttackAbility(abilityData, player, _vfxFactory, _dataService),
                AbilityType.GunShot => new GunShotAbility(abilityData, player, _projectileFactory),
                AbilityType.BulletHell => new BulletHellAbility(abilityData, player, _projectileFactory),
                AbilityType.BuzzSaw => new BuzzSawAbility(abilityData, player, _projectileFactory),
                AbilityType.PuncturedTank => new PuncturedTankAbility(abilityData, player, _projectileFactory),
                AbilityType.EnergyLine => new EnergyLineAbility(abilityData, player, _projectileFactory),

                AbilityType.AntiGravity => new AntiGravityAbility(abilityData, player, _vfxFactory),
                AbilityType.EnergyShield => new EnergyShieldAbility(abilityData, player, _vfxFactory),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}