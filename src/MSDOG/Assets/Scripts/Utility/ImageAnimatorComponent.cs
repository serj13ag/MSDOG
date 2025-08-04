using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    public class ImageAnimatorComponent : BaseSpriteAnimatorComponent
    {
        [SerializeField] private Image _image;

        protected override void ChangeSprite(Sprite sprite)
        {
            _image.sprite = sprite;
        }
    }
}