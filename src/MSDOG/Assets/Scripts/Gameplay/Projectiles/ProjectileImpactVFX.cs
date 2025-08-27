using System;
using UnityEngine;
using Utility;
using Utility.Pools;

namespace Gameplay.Projectiles
{
    public class ProjectileImpactVFX : BasePooledObject
    {
        [SerializeField] private SpriteAnimatorComponent _spriteAnimatorComponent;

        public override void OnGet()
        {
            base.OnGet();

            _spriteAnimatorComponent.OnAnimationFinished += OnAnimationFinished;
        }

        public void Play()
        {
            _spriteAnimatorComponent.Activate();
        }

        private void OnAnimationFinished(object sender, EventArgs e)
        {
            Release();
        }

        protected override void Cleanup()
        {
            base.Cleanup();

            _spriteAnimatorComponent.OnAnimationFinished -= OnAnimationFinished;
        }
    }
}