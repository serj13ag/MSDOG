using Data;
using DTO;
using Services.Gameplay;
using UnityEngine;

namespace Core.Abilities
{
    public class PuncturedTankAbility : BaseCooldownAbility
    {
        private readonly AbilityData _abilityData;
        private readonly Player _player;
        private readonly ProjectileFactory _projectileFactory;

        public PuncturedTankAbility(AbilityData abilityData, Player player, ProjectileFactory projectileFactory)
            : base(abilityData.Cooldown, abilityData.FirstCooldownReduction)
        {
            _abilityData = abilityData;
            _player = player;
            _projectileFactory = projectileFactory;
        }

        protected override void InvokeAction()
        {
            var createProjectileDto = new CreateProjectileDto(_player.transform.position, Vector3.zero, _player, _abilityData);
            _projectileFactory.CreatePlayerPuddleProjectile(createProjectileDto);
        }
    }
}