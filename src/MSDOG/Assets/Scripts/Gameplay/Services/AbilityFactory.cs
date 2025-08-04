using System;
using Core.Controllers;
using Core.Models.Data;
using Core.Services;
using Gameplay.Abilities;

namespace Gameplay.Services
{
    public class AbilityFactory
    {
        private readonly ProjectileFactory _projectileFactory;
        private readonly VfxFactory _vfxFactory;
        private readonly DataService _dataService;
        private readonly SoundService _soundService;

        public AbilityFactory(ProjectileFactory projectileFactory, VfxFactory vfxFactory, DataService dataService,
            SoundService soundService)
        {
            _dataService = dataService;
            _soundService = soundService;
            _projectileFactory = projectileFactory;
            _vfxFactory = vfxFactory;
        }

        public IAbility CreateAbility(AbilityData abilityData, Player player)
        {
            return abilityData.AbilityType switch
            {
                AbilityType.CuttingBlow => new CuttingBlowAbility(abilityData, player, _vfxFactory, _dataService, _soundService),
                AbilityType.RoundAttack => new RoundAttackAbility(abilityData, player, _vfxFactory, _dataService, _soundService),
                AbilityType.GunShot => new GunShotAbility(abilityData, player, _projectileFactory, _soundService),
                AbilityType.BulletHell => new BulletHellAbility(abilityData, player, _projectileFactory, _soundService),
                AbilityType.BuzzSaw => new BuzzSawAbility(abilityData, player, _projectileFactory, _soundService),
                AbilityType.PuncturedTank => new PuncturedTankAbility(abilityData, player, _projectileFactory, _soundService),
                AbilityType.EnergyLine => new EnergyLineAbility(abilityData, player, _projectileFactory, _soundService),

                AbilityType.AntiGravity => new AntiGravityAbility(abilityData, player, _vfxFactory, _soundService),
                AbilityType.EnergyShield => new EnergyShieldAbility(abilityData, player, _vfxFactory, _soundService),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}