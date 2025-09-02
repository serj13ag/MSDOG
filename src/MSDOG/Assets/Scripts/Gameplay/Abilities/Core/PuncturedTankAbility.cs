using Core.Controllers;
using Core.Models.Data;
using Gameplay.Factories;
using Gameplay.Projectiles;
using UnityEngine;

namespace Gameplay.Abilities.Core
{
    public class PuncturedTankAbility : BaseCooldownAbility
    {
        private readonly AbilityData _abilityData;
        private readonly Player _player;
        private readonly IProjectileFactory _projectileFactory;

        public PuncturedTankAbility(AbilityData abilityData, Player player, IProjectileFactory projectileFactory,
            ISoundController soundController)
            : base(abilityData, soundController)
        {
            _abilityData = abilityData;
            _player = player;
            _projectileFactory = projectileFactory;
        }

        protected override void InvokeAction()
        {
            var projectileSpawnData = new ProjectileSpawnData(_player.GetAbilitySpawnPosition(_abilityData.AbilityType), Vector3.zero, _abilityData);
            _projectileFactory.CreateAbilityProjectile(projectileSpawnData);
        }
    }
}