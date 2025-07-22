using System;
using JetBrains.Annotations;
using UnityEngine;

namespace UtilityComponents
{
    public class AnimatorEventsProvider : MonoBehaviour
    {
        public event Action OnAnimationAttackHit;

        [UsedImplicitly]
        public void OnAttackHit()
        {
            OnAnimationAttackHit?.Invoke();
        }
    }
}