using Gameplay.Controllers;
using Gameplay.Interfaces;
using UnityEngine;
using VContainer;

namespace Utility
{
    public class SpriteAnimatorComponent : BaseSpriteAnimatorComponent, IUpdatable
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private IGameplayUpdateController _updateController;

        [Inject]
        public void Construct(IGameplayUpdateController updateController)
        {
            _updateController = updateController;

            updateController.Register(this);
        }

        public void OnUpdate(float deltaTime)
        {
            Tick(deltaTime);
        }

        protected override void ChangeSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }

        private void OnDestroy()
        {
            _updateController.Remove(this);
        }
    }
}