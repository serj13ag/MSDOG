using Core.Controllers;
using Core.Sounds;
using Gameplay.Controllers;
using Gameplay.Interfaces;
using Gameplay.Providers;
using Gameplay.Services;
using GameplayTvHud.Mediators;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace GameplayTvHud.UI
{
    public class CraftButtonHud : MonoBehaviour, IUpdatable
    {
        [SerializeField] private Button _button;
        [SerializeField] private float _buttonOscSpeed = 10f;
        [SerializeField] private float _minScale = 0.8f;
        [SerializeField] private float _maxScale = 1.1f;

        private IGameplayUpdateController _gameplayUpdateController;
        private IPlayerProvider _playerProvider;
        private ISoundController _soundController;
        private IDetailMediator _detailMediator;
        private ITutorialService _tutorialService;

        private bool _canCraft;
        private float _oscTimer;

        [Inject]
        public void Construct(IGameplayUpdateController gameplayUpdateController, IPlayerProvider playerProvider,
            ISoundController soundController, IDetailMediator detailMediator, ITutorialService tutorialService)
        {
            _tutorialService = tutorialService;
            _detailMediator = detailMediator;
            _soundController = soundController;
            _playerProvider = playerProvider;
            _gameplayUpdateController = gameplayUpdateController;

            gameplayUpdateController.Register(this);
        }

        private void Awake()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        public void OnUpdate(float deltaTime)
        {
            if (_canCraft)
            {
                _oscTimer += deltaTime * _buttonOscSpeed;

                var t = (Mathf.Sin(_oscTimer) + 1f) / 2f;
                var scale = Mathf.Lerp(_minScale, _maxScale, t);

                _button.transform.localScale = Vector3.one * scale;
            }
            else
            {
                _button.transform.localScale = Vector3.one;
            }
        }

        public void SetCanCraft(bool canCraft)
        {
            _canCraft = canCraft;

            _button.interactable = canCraft;

            if (canCraft)
            {
                _soundController.PlaySfx(SfxType.CanCraft);
                _tutorialService.OnCanCraft();
            }
        }

        private void OnButtonClick()
        {
            _detailMediator.CraftDetail();
            _playerProvider.Player.ResetExperience();
            _soundController.PlaySfx(SfxType.Craft);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClick);

            _gameplayUpdateController.Remove(this);
        }
    }
}