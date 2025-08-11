using Core.Controllers;
using Core.Models.Data;
using Gameplay.Factories;

namespace Gameplay.Abilities
{
    public class AntiGravityAbility : BasePersistentAbility
    {
        private readonly IAbilityEffectFactory _abilityEffectFactory;

        private readonly Player _player;
        private readonly float _speed;
        private readonly FollowingAbilityEffect _followingAbilityEffectPrefab;

        private FollowingAbilityEffect _followingAbilityEffect;

        public AntiGravityAbility(AbilityData abilityData, Player player, IAbilityEffectFactory abilityEffectFactory,
            ISoundController soundController)
            : base(abilityData, soundController)
        {
            _abilityEffectFactory = abilityEffectFactory;

            _player = player;
            _speed = abilityData.Speed;
            _followingAbilityEffectPrefab = abilityData.FollowingAbilityEffectPrefab;
        }

        protected override void OnActivated()
        {
            _player.ChangeAdditionalSpeed(_speed);

            _followingAbilityEffect = _abilityEffectFactory.CreateFollowingEffect(_followingAbilityEffectPrefab, _player);
        }

        protected override void OnDeactivated()
        {
            _player.ChangeAdditionalSpeed(-_speed);

            _followingAbilityEffect.Clear();
            _followingAbilityEffect = null;
        }
    }
}