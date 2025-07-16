using Data;
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

        public GunShotAbility(AbilityData abilityData, Player player, ProjectileFactory projectileFactory)
            : base(abilityData.Cooldown)
        {
            _player = player;
            _projectileFactory = projectileFactory;

            _damage = abilityData.Damage;
            _speed = abilityData.Speed;
            _pierce = abilityData.Pierce;
        }

        protected override void InvokeAction()
        {
            var createProjectileDto =
                new CreateProjectileDto(_player.transform.position, _player.transform.forward, _damage, _speed, _pierce);
            _projectileFactory.CreatePlayerProjectile(createProjectileDto);
        }
    }
}