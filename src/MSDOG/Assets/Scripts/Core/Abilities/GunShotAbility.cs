using Core.Projectiles;
using Data;
using Services;
using Services.Gameplay;

namespace Core.Abilities
{
    public class GunShotAbility : BaseCooldownAbility
    {
        private readonly AbilityData _abilityData;
        private readonly Player _player;
        private readonly ProjectileFactory _projectileFactory;

        public GunShotAbility(AbilityData abilityData, Player player, ProjectileFactory projectileFactory,
            SoundService soundService)
            : base(abilityData, soundService)
        {
            _abilityData = abilityData;
            _player = player;
            _projectileFactory = projectileFactory;
        }

        protected override void InvokeAction()
        {
            var projectileSpawnData = new ProjectileSpawnData(_player.GetAbilitySpawnPosition(_abilityData.AbilityType), _player.transform.forward, _player, _abilityData);
            _projectileFactory.CreatePlayerGunShotProjectile(projectileSpawnData);
        }
    }
}