using Data;
using DTO;
using Services.Gameplay;
using UnityEngine;

namespace Core.Abilities
{
    public class BulletHellAbility : BaseCooldownAbility
    {
        private readonly Player _player;
        private readonly ProjectileFactory _projectileFactory;
        private readonly int _damage;
        private readonly float _speed;
        private readonly int _pierce;

        public BulletHellAbility(AbilityData abilityData, Player player, ProjectileFactory projectileFactory)
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
            Vector3 randomDirection;
            do
            {
                randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            } while (randomDirection == Vector3.zero);

            randomDirection.Normalize();

            var createProjectileDto =
                new CreateProjectileDto(_player.transform.position, randomDirection, _damage, _speed, _pierce);
            _projectileFactory.CreatePlayerProjectile(createProjectileDto);
        }
    }
}