using Core.Controllers;
using Core.Models.Data;
using Gameplay.Factories;
using Gameplay.Projectiles;

namespace Gameplay.Abilities
{
    public class GunShotAbility : BaseCooldownAbility
    {
        private readonly AbilityData _abilityData;
        private readonly Player _player;
        private readonly ProjectileFactory _projectileFactory;

        public GunShotAbility(AbilityData abilityData, Player player, ProjectileFactory projectileFactory,
            ISoundController soundController)
            : base(abilityData, soundController)
        {
            _abilityData = abilityData;
            _player = player;
            _projectileFactory = projectileFactory;
        }

        protected override void InvokeAction()
        {
            var projectileSpawnData = new ProjectileSpawnData(_player.GetAbilitySpawnPosition(_abilityData.AbilityType), _player.transform.forward, _abilityData);
            _projectileFactory.CreateAbilityProjectile(projectileSpawnData);
        }
    }
}