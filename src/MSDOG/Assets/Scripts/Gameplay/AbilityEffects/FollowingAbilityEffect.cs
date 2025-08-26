using Gameplay.Controllers;
using Gameplay.Interfaces;
using UnityEngine;
using VContainer;

namespace Gameplay.AbilityEffects
{
    public class FollowingAbilityEffect : BaseAbilityEffect, IUpdatable
    {
        private IGameplayUpdateController _updateController;

        private Transform _followTarget;

        [Inject]
        public void Construct(IGameplayUpdateController updateController)
        {
            _updateController = updateController;
        }

        public override void OnGet()
        {
            base.OnGet();

            _updateController.Register(this);
        }

        public void Init(Transform followTarget)
        {
            _followTarget = followTarget;
        }

        public void OnUpdate(float deltaTime)
        {
            if (!_followTarget)
            {
                return;
            }

            transform.position = _followTarget.transform.position;
        }

        public void Clear()
        {
            Release();
        }

        protected override void Cleanup()
        {
            base.Cleanup();

            _followTarget = null;
            _updateController.Remove(this);
        }
    }
}