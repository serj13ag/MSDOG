using System;
using UnityEngine;
using Utility;

namespace Gameplay.AbilityVFX
{
    public class OneTimeAbilityVFX : BaseAbilityVFX
    {
        [SerializeField] private SpriteAnimatorComponent _spriteAnimatorComponent;

        public void Init(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            transform.position = position;
            transform.rotation = rotation;
            transform.localScale = scale;

            _spriteAnimatorComponent.Activate();
        }

        public override void OnGet()
        {
            base.OnGet();

            _spriteAnimatorComponent.OnAnimationFinished += OnAnimationFinished;
        }

        public override void OnRelease()
        {
            base.OnRelease();

            _spriteAnimatorComponent.OnAnimationFinished -= OnAnimationFinished;
        }

        private void OnAnimationFinished(object sender, EventArgs e)
        {
            Release();
        }
    }
}