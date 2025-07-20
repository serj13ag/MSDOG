using UnityEngine;

namespace Core
{
    public class AnimationBlock
    {
        private static readonly int RunningBoolKey = Animator.StringToHash("Running");

        private readonly Animator _animator;

        public AnimationBlock(Animator animator)
        {
            _animator = animator;
        }

        public void SetRunning(bool value)
        {
            _animator.SetBool(RunningBoolKey, value);
        }
    }
}