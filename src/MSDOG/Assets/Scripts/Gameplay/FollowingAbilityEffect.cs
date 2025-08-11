using UnityEngine;
using Utility;

namespace Gameplay
{
    public class FollowingAbilityEffect : BasePooledObject
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