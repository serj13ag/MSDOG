using UnityEngine;
using Utility;
using Utility.Pools;

namespace Gameplay.Projectiles
{
    public class ProjectileImpactVFX : BasePooledObject
    {
        [SerializeField] private SpriteAnimatorComponent _spriteAnimatorComponent;

        public void Play()
        {
            _spriteAnimatorComponent.Activate();
        }
    }
}