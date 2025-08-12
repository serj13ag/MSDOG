using UnityEngine;

namespace Gameplay.AbilityEffects
{
    public class FollowingAbilityEffect : BaseAbilityEffect
    {
        private Transform _followTarget;

        public void Init(Transform followTarget)
        {
            _followTarget = followTarget;
        }

        private void Update()
        {
            transform.position = _followTarget.transform.position;
        }

        public override void OnRelease()
        {
            base.OnRelease();

            _followTarget = null;
        }

        public void Clear()
        {
            Release();
        }
    }
}