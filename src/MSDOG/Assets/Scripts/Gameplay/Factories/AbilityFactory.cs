using System;
using Core.Controllers;
using Core.Models.Data;
using Core.Services;
using Gameplay.Abilities.Core;
using Gameplay.Abilities.View;
using Gameplay.Interfaces;

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

        public IAbility CreateAbility(AbilityData abilityData, IEntityWithAbilities entityWithAbilities)
        {
            IAbility ability;
            switch (abilityData.AbilityType)
            {
                case AbilityType.CuttingBlow:
                {
                    var cuttingBlowAbility = new CuttingBlowAbility(abilityData, entityWithAbilities, _dataService);
                    _ = new OneTimeAbilityPresenter(entityWithAbilities, cuttingBlowAbility, abilityData, _abilityVFXFactory,
                        _soundController);
                    ability = cuttingBlowAbility;
                    break;
                }
                case AbilityType.RoundAttack:
                {
                    var roundAttackAbility = new RoundAttackAbility(abilityData, entityWithAbilities, _dataService);
                    _ = new OneTimeAbilityPresenter(entityWithAbilities, roundAttackAbility, abilityData, _abilityVFXFactory,
                        _soundController);
                    ability = roundAttackAbility;
                    break;
                }
                case AbilityType.GunShot:
                    ability = new GunShotAbility(abilityData, entityWithAbilities, _projectileFactory);
                    break;
                case AbilityType.BulletHell:
                    ability = new BulletHellAbility(abilityData, entityWithAbilities, _projectileFactory);
                    break;
                case AbilityType.BuzzSaw:
                    ability = new BuzzSawAbility(abilityData, entityWithAbilities, _projectileFactory);
                    break;
                case AbilityType.PuncturedTank:
                    ability = new PuncturedTankAbility(abilityData, entityWithAbilities, _projectileFactory);
                    break;
                case AbilityType.EnergyLine:
                    ability = new EnergyLineAbility(abilityData, entityWithAbilities, _projectileFactory);
                    break;
                case AbilityType.AntiGravity:
                {
                    ability = new AntiGravityAbility(abilityData, entityWithAbilities);
                    _ = new FollowingAbilityPresenter(entityWithAbilities, ability, abilityData, _abilityVFXFactory,
                        _soundController);
                    break;
                }
                case AbilityType.EnergyShield:
                {
                    ability = new EnergyShieldAbility(abilityData, entityWithAbilities);
                    _ = new FollowingAbilityPresenter(entityWithAbilities, ability, abilityData, _abilityVFXFactory,
                        _soundController);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return ability;
        }
    }
}