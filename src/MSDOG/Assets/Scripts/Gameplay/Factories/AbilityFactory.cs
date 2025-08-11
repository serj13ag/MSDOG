using System;
using Core.Controllers;
using Core.Models.Data;
using Core.Services;
using Gameplay.Abilities;

namespace Gameplay.Factories
{
    public class AbilityFactory : IAbilityFactory
    {
        private readonly IProjectileFactory _projectileFactory;
        private readonly IVfxFactory _vfxFactory;
        private readonly IDataService _dataService;
        private readonly ISoundController _soundController;
        private readonly IAbilityEffectFactory _abilityEffectFactory;

        public AbilityFactory(IProjectileFactory projectileFactory, IVfxFactory vfxFactory, IDataService dataService,
            ISoundController soundController, IAbilityEffectFactory abilityEffectFactory)
        {
            _dataService = dataService;
            _soundController = soundController;
            _abilityEffectFactory = abilityEffectFactory;
            _projectileFactory = projectileFactory;
            _vfxFactory = vfxFactory;
        }

        public IAbility CreateAbility(AbilityData abilityData, Player player)
        {
            return abilityData.AbilityType switch
            {
                AbilityType.CuttingBlow => new CuttingBlowAbility(abilityData, player, _vfxFactory, _dataService, _soundController),
                AbilityType.RoundAttack => new RoundAttackAbility(abilityData, player, _vfxFactory, _dataService, _soundController),
                AbilityType.GunShot => new GunShotAbility(abilityData, player, _projectileFactory, _soundController),
                AbilityType.BulletHell => new BulletHellAbility(abilityData, player, _projectileFactory, _soundController),
                AbilityType.BuzzSaw => new BuzzSawAbility(abilityData, player, _projectileFactory, _soundController),
                AbilityType.PuncturedTank => new PuncturedTankAbility(abilityData, player, _projectileFactory, _soundController),
                AbilityType.EnergyLine => new EnergyLineAbility(abilityData, player, _projectileFactory, _soundController),

                AbilityType.AntiGravity => new AntiGravityAbility(abilityData, player, _abilityEffectFactory, _soundController),
                AbilityType.EnergyShield => new EnergyShieldAbility(abilityData, player, _abilityEffectFactory, _soundController),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}