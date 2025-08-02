using Core.Projectiles;
using Data;
using Services;
using Services.Gameplay;
using UnityEngine;

namespace Core.Abilities
{
    public class PuncturedTankAbility : BaseCooldownAbility
    {
        private readonly AbilityData _abilityData;
        private readonly Player _player;
        private readonly ProjectileFactory _projectileFactory;

        public PuncturedTankAbility(AbilityData abilityData, Player player, ProjectileFactory projectileFactory,
            SoundService soundService)
            : base(abilityData, soundService)
        {
            _abilityData = abilityData;
            _player = player;
            _projectileFactory = projectileFactory;
        }

        protected override void InvokeAction()
        {
            var projectileSpawnData = new ProjectileSpawnData(_player.GetAbilitySpawnPosition(_abilityData.AbilityType), Vector3.zero, _player, _abilityData);
            _projectileFactory.CreateAbilityProjectile(projectileSpawnData);
        }
    }
}