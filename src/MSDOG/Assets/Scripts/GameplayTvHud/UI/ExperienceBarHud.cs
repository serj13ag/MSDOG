using System;
using Core.Controllers;
using Core.Sounds;
using Gameplay.Controllers;
using Gameplay.Interfaces;
using Gameplay.Providers;
using Gameplay.Services;
using GameplayTvHud.Mediators;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace GameplayTvHud.UI
{
    public class ExperienceBarHud : MonoBehaviour, IUpdatable, IDisposable
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _fillImage;
        [SerializeField] private Button _craftButton;
        [SerializeField] private float _buttonOscSpeed = 10f;
        [SerializeField] private float _minScale = 0.8f;
        [SerializeField] private float _maxScale = 1.1f;

        private IPlayerProvider _playerProvider;
        private ISoundController _soundController;
        private IGameplayUpdateController _gameplayUpdateController;
        private ITutorialService _tutorialService;
        private IDetailMediator _detailMediator;

        private bool _canCraft;
        private float _oscTimer;

        [Inject]
        public void Construct(IPlayerProvider playerProvider, ISoundController soundController, ITutorialService tutorialService,
            IGameplayUpdateController gameplayUpdateController, IDetailMediator detailMediator)
        {
            _detailMediator = detailMediator;
            _playerProvider = playerProvider;
            _tutorialService = tutorialService;
            _soundController = soundController;
            _gameplayUpdateController = gameplayUpdateController;

            UpdateView();

            gameplayUpdateController.Register(this);
            playerProvider.Player.OnExperienceChanged += OnPlayerExperienceChanged;
        }

        private void Awake()
        {
            _craftButton.onClick.AddListener(OnCraftButtonClick);
        }

        public void OnUpdate(float deltaTime)
        {
            if (_canCraft)
            {
                _oscTimer += deltaTime * _buttonOscSpeed;

                var t = (Mathf.Sin(_oscTimer) + 1f) / 2f;
                var scale = Mathf.Lerp(_minScale, _maxScale, t);

                _craftButton.transform.localScale = Vector3.one * scale;
            }
            else
            {
                _craftButton.transform.localScale = Vector3.one;
            }
        }

        private void OnCraftButtonClick()
        {
            _detailMediator.CraftDetail();
            _playerProvider.Player.ResetExperience();
            UpdateView();

            _soundController.PlaySfx(SfxType.Craft);
        }

        private void OnPlayerExperienceChanged()
        {
            _soundController.PlaySfx(SfxType.DetailUp);

            UpdateView();
        }

        private void UpdateView()
        {
            var player = _playerProvider.Player;

            _text.text = $"{player.CurrentExperience}/{player.MaxExperience}";
            _fillImage.fillAmount = (float)player.CurrentExperience / player.MaxExperience;

            _canCraft = player.CurrentExperience >= player.MaxExperience;
            _craftButton.interactable = _canCraft;

            if (_canCraft)
            {
                _soundController.PlaySfx(SfxType.CanCraft);
                _tutorialService.OnCanCraft();
            }
        }

        private void OnDestroy()
        {
            _craftButton.onClick.RemoveListener(OnCraftButtonClick);
            _playerProvider.Player.OnExperienceChanged -= OnPlayerExperienceChanged;
        }

        public void Dispose()
        {
            _gameplayUpdateController.Remove(this);
        }
    }
}