using Core.Projectiles;
using Data;
using Services;
using Services.Gameplay;
using UnityEngine;

namespace Core.Abilities
{
    public class EnergyLineAbility : BaseCooldownAbility
    {
        private readonly AbilityData _abilityData;
        private readonly Player _player;
        private readonly ProjectileFactory _projectileFactory;

        public EnergyLineAbility(AbilityData abilityData, Player player, ProjectileFactory projectileFactory,
            SoundService soundService)
            : base(abilityData, soundService)
        {
            _abilityData = abilityData;
            _player = player;
            _projectileFactory = projectileFactory;
        }

        protected override void InvokeAction()
        {
            Vector3 randomDirection;
            do
            {
                randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            } while (randomDirection == Vector3.zero);

            randomDirection.Normalize();

            var projectileSpawnData = new ProjectileSpawnData(_player.GetAbilitySpawnPosition(_abilityData.AbilityType), randomDirection, _player, _abilityData);
            _projectileFactory.CreateAbilityProjectile(projectileSpawnData);
        }
    }
}