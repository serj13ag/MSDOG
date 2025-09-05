using System;
using UnityEngine;

namespace Utility
{
    public abstract class BaseSpriteAnimatorComponent : MonoBehaviour
    {
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private float _timePerFrame;
        [SerializeField] private bool _isLooped;
        [SerializeField] private bool _destroyOnFinish;
        [SerializeField] private bool _deactivateOnStart;

        private bool _isActive;
        private float _timeTillChangeSprite;
        private int _frameIndex;

        public event EventHandler<EventArgs> OnAnimationFinished;

        private void Start()
        {
            if (_deactivateOnStart)
            {
                return;
            }

            _isActive = true;
            ResetAndUpdateSprite();
        }

        public void Activate()
        {
            if (_isActive)
            {
                return;
            }

            _isActive = true;
            ResetAndUpdateSprite();
        }

        public void Deactivate()
        {
            if (!_isActive)
            {
                return;
            }

            _isActive = false;
            ResetAndUpdateSprite();
        }

        protected void Tick(float deltaTime)
        {
            if (!_isActive)
            {
                return;
            }

            if (_timeTillChangeSprite > 0f)
            {
                _timeTillChangeSprite -= deltaTime;
            }
            else
            {
                if (_sprites.Length - 1 <= _frameIndex)
                {
                    LoopEnded();
                }
                else
                {
                    ResetTimer();
                    _frameIndex++;
                    ChangeSprite(_sprites[_frameIndex]);
                }
            }
        }

        protected abstract void ChangeSprite(Sprite sprite);

        private void LoopEnded()
        {
            if (_isLooped)
            {
                ResetAndUpdateSprite();
            }
            else
            {
                _isActive = false;
                ChangeSprite(null);

                OnAnimationFinished?.Invoke(this, EventArgs.Empty);

                if (_destroyOnFinish)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void ResetAndUpdateSprite()
        {
            _frameIndex = 0;
            ChangeSprite(_sprites[_frameIndex]);
            ResetTimer();
        }

        private void ResetTimer()
        {
            _timeTillChangeSprite = _timePerFrame;
        }
    }
}