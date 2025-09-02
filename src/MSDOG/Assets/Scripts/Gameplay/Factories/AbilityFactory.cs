using System;
using Core.Controllers;
using Core.Models.Data;
using Core.Services;
using Gameplay.Abilities;
using Gameplay.AbilityVFX;

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
                    ability = new CuttingBlowAbility(abilityData, player, _dataService, _soundController);
                    _ = new OneTimeAbilityPresenter(player, ability, abilityData, _abilityVFXFactory);
                    break;
                }
                case AbilityType.RoundAttack:
                {
                    ability = new RoundAttackAbility(abilityData, player, _dataService, _soundController);
                    _ = new OneTimeAbilityPresenter(player, ability, abilityData, _abilityVFXFactory);
                    break;
                }
                case AbilityType.GunShot:
                    ability = new GunShotAbility(abilityData, player, _projectileFactory, _soundController);
                    break;
                case AbilityType.BulletHell:
                    ability = new BulletHellAbility(abilityData, player, _projectileFactory, _soundController);
                    break;
                case AbilityType.BuzzSaw:
                    ability = new BuzzSawAbility(abilityData, player, _projectileFactory, _soundController);
                    break;
                case AbilityType.PuncturedTank:
                    ability = new PuncturedTankAbility(abilityData, player, _projectileFactory, _soundController);
                    break;
                case AbilityType.EnergyLine:
                    ability = new EnergyLineAbility(abilityData, player, _projectileFactory, _soundController);
                    break;
                case AbilityType.AntiGravity:
                {
                    ability = new AntiGravityAbility(abilityData, player, _soundController);
                    _ = new FollowingAbilityPresenter(player, ability, abilityData, _abilityVFXFactory);
                    break;
                }
                case AbilityType.EnergyShield:
                {
                    ability = new EnergyShieldAbility(abilityData, player, _soundController);
                    _ = new FollowingAbilityPresenter(player, ability, abilityData, _abilityVFXFactory);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return ability;
        }
    }
}