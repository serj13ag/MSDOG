using UnityEngine;

namespace Utility
{
    public class SpriteAnimatorComponent : BaseSpriteAnimatorComponent
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        protected override void ChangeSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }
    }
}