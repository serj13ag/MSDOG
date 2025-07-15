using System;
using Core;
using Core.Abilities;
using Core.Details;

namespace Services.Gameplay
{
    public class AbilityFactory
    {
        private readonly ProjectileFactory _projectileFactory;

        public AbilityFactory(ProjectileFactory projectileFactory)
        {
            _projectileFactory = projectileFactory;
        }

        public IAbility CreateAbility(Detail detail, Player player)
        {
            return detail.Type switch
            {
                DetailType.CuttingBlow => CreateCuttingBlowAbility(player),
                DetailType.GunShot => CreateGunShotAbility(player),
                DetailType.BulletHell => CreateBulletHellAbility(player),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        public IAbility CreateCuttingBlowAbility(Player player)
        {
            return new CuttingBlowAbility(player);
        }

        private IAbility CreateGunShotAbility(Player player)
        {
            return new GunShotAbility(player, _projectileFactory);
        }

        private IAbility CreateBulletHellAbility(Player player)
        {
            return new BulletHellAbility(player, _projectileFactory);
        }
    }
}