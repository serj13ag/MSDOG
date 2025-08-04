using Core.Controllers;
using Core.Models.Data;
using Gameplay.Factories;
using Gameplay.Projectiles;
using UnityEngine;

namespace Gameplay.Abilities
{
    public class PuncturedTankAbility : BaseCooldownAbility
    {
        private readonly AbilityData _abilityData;
        private readonly Player _player;
        private readonly ProjectileFactory _projectileFactory;

        public PuncturedTankAbility(AbilityData abilityData, Player player, ProjectileFactory projectileFactory,
            SoundController soundController)
            : base(abilityData, soundController)
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