using Data;
using Services.Gameplay;
using VFX;

namespace Core.Abilities
{
    public class AntiGravityAbility : BasePersistentAbility
    {
        private readonly VfxFactory _vfxFactory;
        private readonly Player _player;
        private readonly float _speed;

        private FollowingAbilityEffect _followingAbilityEffect;

        public AntiGravityAbility(AbilityData abilityData, Player player, VfxFactory vfxFactory)
        {
            _vfxFactory = vfxFactory;
            _player = player;
            _speed = abilityData.Speed;
        }

        protected override void OnActivated()
        {
            _player.ChangeAdditionalSpeed(_speed);

            _followingAbilityEffect = _vfxFactory.CreateAntiGravityEffect(_player);
        }

        protected override void OnDeactivated()
        {
            _player.ChangeAdditionalSpeed(-_speed);

            _followingAbilityEffect.Clear();
            _followingAbilityEffect = null;
        }
    }
}