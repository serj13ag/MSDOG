using System;
using Core.Controllers;
using Core.Models.Data;
using Core.Services;
using Gameplay.Abilities.Core;
using Gameplay.Abilities.View;

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
            IAbility ability;
            switch (abilityData.AbilityType)
            {
                case AbilityType.CuttingBlow:
                {
                    var cuttingBlowAbility = new CuttingBlowAbility(abilityData, player, _dataService);
                    _ = new OneTimeAbilityPresenter(player, cuttingBlowAbility, abilityData, _abilityVFXFactory, _soundController);
                    ability = cuttingBlowAbility;
                    break;
                }
                case AbilityType.RoundAttack:
                {
                    var roundAttackAbility = new RoundAttackAbility(abilityData, player, _dataService);
                    _ = new OneTimeAbilityPresenter(player, roundAttackAbility, abilityData, _abilityVFXFactory, _soundController);
                    ability = roundAttackAbility;
                    break;
                }
                case AbilityType.GunShot:
                    ability = new GunShotAbility(abilityData, player, _projectileFactory);
                    break;
                case AbilityType.BulletHell:
                    ability = new BulletHellAbility(abilityData, player, _projectileFactory);
                    break;
                case AbilityType.BuzzSaw:
                    ability = new BuzzSawAbility(abilityData, player, _projectileFactory);
                    break;
                case AbilityType.PuncturedTank:
                    ability = new PuncturedTankAbility(abilityData, player, _projectileFactory);
                    break;
                case AbilityType.EnergyLine:
                    ability = new EnergyLineAbility(abilityData, player, _projectileFactory);
                    break;
                case AbilityType.AntiGravity:
                {
                    ability = new AntiGravityAbility(abilityData, player);
                    _ = new FollowingAbilityPresenter(player, ability, abilityData, _abilityVFXFactory, _soundController);
                    break;
                }
                case AbilityType.EnergyShield:
                {
                    ability = new EnergyShieldAbility(abilityData, player);
                    _ = new FollowingAbilityPresenter(player, ability, abilityData, _abilityVFXFactory, _soundController);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return ability;
        }
    }
}