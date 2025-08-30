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
        private readonly IDataService _dataService;
        private readonly ISoundController _soundController;
        private readonly IAbilityVFXFactory _abilityVFXFactory;

        public AbilityFactory(IProjectileFactory projectileFactory, IDataService dataService, ISoundController soundController,
            IAbilityVFXFactory abilityVFXFactory)
        {
            _dataService = dataService;
            _soundController = soundController;
            _abilityVFXFactory = abilityVFXFactory;
            _projectileFactory = projectileFactory;
        }

        public IAbility CreateAbility(AbilityData abilityData, Player player)
        {
            return abilityData.AbilityType switch
            {
                AbilityType.CuttingBlow => new CuttingBlowAbility(abilityData, player, _abilityVFXFactory, _dataService,
                    _soundController),
                AbilityType.RoundAttack => new RoundAttackAbility(abilityData, player, _abilityVFXFactory, _dataService,
                    _soundController),
                AbilityType.GunShot => new GunShotAbility(abilityData, player, _projectileFactory, _soundController),
                AbilityType.BulletHell => new BulletHellAbility(abilityData, player, _projectileFactory, _soundController),
                AbilityType.BuzzSaw => new BuzzSawAbility(abilityData, player, _projectileFactory, _soundController),
                AbilityType.PuncturedTank => new PuncturedTankAbility(abilityData, player, _projectileFactory, _soundController),
                AbilityType.EnergyLine => new EnergyLineAbility(abilityData, player, _projectileFactory, _soundController),

                AbilityType.AntiGravity => new AntiGravityAbility(abilityData, player, _abilityVFXFactory, _soundController),
                AbilityType.EnergyShield => new EnergyShieldAbility(abilityData, player, _abilityVFXFactory, _soundController),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}