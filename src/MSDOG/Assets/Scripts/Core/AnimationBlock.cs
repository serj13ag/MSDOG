using UnityEngine;

namespace Core
{
    public class AnimationBlock
    {
        private static readonly int RunningBoolKey = Animator.StringToHash("Running");
        private static readonly int AttackTriggerKey = Animator.StringToHash("Attack");

        private readonly Animator _animator;

        public AnimationBlock(Animator animator)
        {
            _animator = animator;
        }

        public void SetRunning(bool value)
        {
            _animator.SetBool(RunningBoolKey, value);
        }

        public void TriggerAttack()
        {
            _animator.SetTrigger(AttackTriggerKey);
        }
    }
}