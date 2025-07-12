using DTO;
using Services.Gameplay;

namespace Core.Abilities
{
    public class GunShotAbility : BaseCooldownAbility
    {
        private readonly Player _player;
        private readonly ProjectileFactory _projectileFactory;
        private readonly int _damage;
        private readonly float _speed;
        private readonly int _pierce;

        public GunShotAbility(Player player, ProjectileFactory projectileFactory)
            : base(cooldown: 1f)
        {
            _player = player;
            _projectileFactory = projectileFactory;

            _damage = 1;
            _speed = 5f;
            _pierce = 1;
        }

        protected override void InvokeAction()
        {
            var createProjectileDto =
                new CreateProjectileDto(_player.transform.position, _player.transform.forward, _damage, _speed, _pierce);
            _projectileFactory.CreatePlayerProjectile(createProjectileDto);
        }
    }
}