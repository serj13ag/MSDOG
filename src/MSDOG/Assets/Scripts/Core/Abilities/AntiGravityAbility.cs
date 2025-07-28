using Data;
using Services.Gameplay;
using UnityEngine;
using VFX;

namespace Core.Abilities
{
    public class AntiGravityAbility : IAbility
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

        public void Activate()
        {
            _player.ChangeAdditionalSpeed(_speed);

            _followingAbilityEffect = _vfxFactory.CreateAntiGravityEffect(_player);
        }

        public void OnUpdate(float deltaTime)
        {
        }

        public void Deactivate()
        {
            _player.ChangeAdditionalSpeed(-_speed);

            _followingAbilityEffect.Clear();
            _followingAbilityEffect = null;
        }
    }
}