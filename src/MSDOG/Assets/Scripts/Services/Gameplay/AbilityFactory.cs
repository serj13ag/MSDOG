using Core;
using Core.Abilities;

namespace Services.Gameplay
{
    public class AbilityFactory
    {
        private readonly ProjectileFactory _projectileFactory;

        public AbilityFactory(ProjectileFactory projectileFactory)
        {
            _projectileFactory = projectileFactory;
        }

        public IAbility CreateHorizontalSlashAbility(Player player)
        {
            return new HorizontalSlashAbility(player);
        }

        public IAbility CreateForwardShotAbility(Player player)
        {
            return new ForwardShotAbility(player, _projectileFactory);
        }
    }
}