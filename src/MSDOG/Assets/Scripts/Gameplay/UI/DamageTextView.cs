using DG.Tweening;
using TMPro;
using UnityEngine;
using Utility.Pools;

namespace Gameplay.UI
{
    public class DamageTextView : BasePooledObject
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _scaleInDuration = 0.3f;
        [SerializeField] private float _scaleOutDuration = 0.8f;
        [SerializeField] private float _fadeInDuration = 0.1f;
        [SerializeField] private float _fadeOutStart = 0.5f;
        [SerializeField] private float _fadeOutDuration = 0.8f;

        public void Init(Vector3 position, int damageDealt)
        {
            transform.position = position;
            transform.rotation = Quaternion.Euler(90, 0, 0);

            _text.text = damageDealt.ToString();

            transform.localScale = Vector3.zero;
            _canvasGroup.alpha = 0;

            DOTween.Sequence()
                .Append(transform.DOScale(Vector3.one, _scaleInDuration))
                .Append(transform.DOScale(Vector3.zero, _scaleOutDuration))
                .Insert(0f, _canvasGroup.DOFade(1, _fadeInDuration))
                .Insert(_fadeOutStart, _canvasGroup.DOFade(0, _fadeOutDuration))
                .OnComplete(Release);
        }
    }
}