using DG.Tweening;
using Gameplay.Controllers;
using Gameplay.Interfaces;
using TMPro;
using UnityEngine;
using Utility.Pools;
using VContainer;

namespace Gameplay.UI
{
    public class DamageTextView : BasePooledObject, IUpdatable
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _scaleInDuration = 0.3f;
        [SerializeField] private float _scaleOutDuration = 0.8f;
        [SerializeField] private float _fadeInDuration = 0.1f;
        [SerializeField] private float _fadeOutStart = 0.5f;
        [SerializeField] private float _fadeOutDuration = 0.8f;

        private IGameplayUpdateController _gameplayUpdateController;

        private Sequence _sequence;

        [Inject]
        public void Construct(IGameplayUpdateController gameplayUpdateController)
        {
            _gameplayUpdateController = gameplayUpdateController;
        }

        public override void OnGet()
        {
            base.OnGet();

            _gameplayUpdateController.Register(this);
        }

        public void Init(Vector3 position, int damageDealt)
        {
            transform.position = position;
            transform.rotation = Quaternion.Euler(90, 0, 0);

            _text.text = damageDealt.ToString();

            transform.localScale = Vector3.zero;
            _canvasGroup.alpha = 0;

            _sequence = DOTween.Sequence()
                .Append(transform.DOScale(Vector3.one, _scaleInDuration))
                .Append(transform.DOScale(Vector3.zero, _scaleOutDuration))
                .Insert(0f, _canvasGroup.DOFade(1, _fadeInDuration))
                .Insert(_fadeOutStart, _canvasGroup.DOFade(0, _fadeOutDuration))
                .SetUpdate(UpdateType.Manual)
                .OnComplete(Release);
        }

        public void OnUpdate(float deltaTime)
        {
            _sequence?.ManualUpdate(deltaTime, deltaTime);
        }

        protected override void Cleanup()
        {
            base.Cleanup();

            _sequence?.Kill();

            _gameplayUpdateController.Remove(this);
        }
    }
}