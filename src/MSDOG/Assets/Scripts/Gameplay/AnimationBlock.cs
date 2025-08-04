using UnityEngine;

namespace Gameplay
{
    public class AnimationBlock
    {
        private static readonly int RunningBoolKey = Animator.StringToHash("Running");
        private static readonly int AttackTriggerKey = Animator.StringToHash("Attack");
        private static readonly int MovelessBoolKey = Animator.StringToHash("Moveless");

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

        public void SetMoveless(bool value)
        {
            _animator.SetBool(MovelessBoolKey, value);
        }
    }
}