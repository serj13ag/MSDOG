using DTO;
using Services.Gameplay;

namespace Core.Abilities
{
    public class ForwardShotAbility : IAbility
    {
        private readonly Player _player;
        private readonly ProjectileFactory _projectileFactory;
        private readonly float _cooldown;
        private readonly int _damage;
        private readonly float _speed;
        private readonly int _pierce;

        private float _timeTillSlash;

        public ForwardShotAbility(Player player, ProjectileFactory projectileFactory)
        {
            _player = player;
            _projectileFactory = projectileFactory;

            _cooldown = 1f;
            _damage = 1;
            _speed = 5f;
            _pierce = 1;

            ResetTimeTillShoot();
        }

        public void OnUpdate(float deltaTime)
        {
            if (_timeTillSlash > 0f)
            {
                _timeTillSlash -= deltaTime;
                return;
            }

            Shoot();
            ResetTimeTillShoot();
        }

        private void Shoot()
        {
            var createProjectileDTO = new CreateProjectileDTO(_player.transform.position, _player.transform.forward, _damage, _speed, _pierce);
            _projectileFactory.CreatePlayerProjectile(createProjectileDTO);
        }

        private void ResetTimeTillShoot()
        {
            _timeTillSlash = _cooldown;
        }
    }
}