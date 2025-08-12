using Core.Controllers;
using Core.Models.Data;
using Gameplay.AbilityEffects;
using Gameplay.Factories;

namespace Gameplay.Abilities
{
    public class AntiGravityAbility : BasePersistentAbility
    {
        private readonly IAbilityEffectFactory _abilityEffectFactory;

        private readonly AbilityData _abilityData;
        private readonly Player _player;
        private readonly float _speed;

        private FollowingAbilityEffect _followingAbilityEffect;

        public AntiGravityAbility(AbilityData abilityData, Player player, IAbilityEffectFactory abilityEffectFactory,
            ISoundController soundController)
            : base(abilityData, soundController)
        {
            _abilityData = abilityData;
            _abilityEffectFactory = abilityEffectFactory;

            _player = player;
            _speed = abilityData.Speed;
        }

        protected override void OnActivated()
        {
            _player.ChangeAdditionalSpeed(_speed);

            _followingAbilityEffect = _abilityEffectFactory.CreateEffect<FollowingAbilityEffect>(_player, _abilityData);
        }

        protected override void OnDeactivated()
        {
            _player.ChangeAdditionalSpeed(-_speed);

            _followingAbilityEffect.Clear();
            _followingAbilityEffect = null;
        }
    }
}