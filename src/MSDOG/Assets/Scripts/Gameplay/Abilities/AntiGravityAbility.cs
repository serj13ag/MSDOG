using Core.Controllers;
using Core.Models.Data;
using Gameplay.AbilityVFX;
using Gameplay.Factories;

namespace Gameplay.Abilities
{
    public class AntiGravityAbility : BasePersistentAbility
    {
        private readonly IAbilityVFXFactory _abilityVFXFactory;

        private readonly AbilityData _abilityData;
        private readonly Player _player;
        private readonly float _speed;

        private FollowingAbilityVFX _followingAbilityVFX; // TODO: separate effects from domain

        public AntiGravityAbility(AbilityData abilityData, Player player, IAbilityVFXFactory abilityVFXFactory,
            ISoundController soundController)
            : base(abilityData, soundController)
        {
            _abilityData = abilityData;
            _abilityVFXFactory = abilityVFXFactory;

            _player = player;
            _speed = abilityData.Speed;
        }

        protected override void OnActivated()
        {
            _player.ChangeAdditionalSpeed(_speed);

            _followingAbilityVFX = _abilityVFXFactory.CreateEffect<FollowingAbilityVFX>(_player, _abilityData);
        }

        protected override void OnDeactivated()
        {
            _player.ChangeAdditionalSpeed(-_speed);

            _followingAbilityVFX.Clear();
            _followingAbilityVFX = null;
        }
    }
}