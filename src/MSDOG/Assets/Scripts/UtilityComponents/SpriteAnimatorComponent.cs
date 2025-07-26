using UnityEngine;

namespace UtilityComponents
{
    public class SpriteAnimatorComponent : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private float _timePerFrame;
        [SerializeField] private bool _isLooped;
        [SerializeField] private bool _destroyOnFinish;

        private bool _isActive;
        private float _timeTillChangeSprite;
        private int _frameIndex;

        private void Start()
        {
            _isActive = true;
            ResetAndUpdateSprite();
        }

        private void Update()
        {
            if (!_isActive)
            {
                return;
            }

            if (_timeTillChangeSprite > 0f)
            {
                _timeTillChangeSprite -= Time.deltaTime;
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

                if (_destroyOnFinish)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void ChangeSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
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