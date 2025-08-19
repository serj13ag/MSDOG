using Core.Controllers;
using Core.Services;
using Core.Sounds;
using Gameplay.Providers;
using Gameplay.Services;
using TMPro;
using UI.HUD.DetailsZone;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI.HUD
{
    public class ExperienceBarHud : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _fillImage;
        [SerializeField] private Button _craftButton;
        [SerializeField] private float _buttonOscSpeed = 10f;
        [SerializeField] private float _minScale = 0.8f;
        [SerializeField] private float _maxScale = 1.1f;

        private IDataService _dataService;
        private IPlayerProvider _playerProvider;
        private ISoundController _soundController;
        private ITutorialService _tutorialService;
        private ILevelFlowService _levelFlowService;

        private DetailsZoneHud _detailsZoneHud;

        private bool _canCraft;
        private float _oscTimer;

        [Inject]
        public void Construct(IDataService dataService, IPlayerProvider playerProvider, ILevelFlowService levelFlowService,
            ISoundController soundController, ITutorialService tutorialService, DetailsZoneHud detailsZoneHud)
        {
            _levelFlowService = levelFlowService;
            _playerProvider = playerProvider;
            _tutorialService = tutorialService;
            _soundController = soundController;
            _dataService = dataService;
            _detailsZoneHud = detailsZoneHud;
        }

        public void Init()
        {
            UpdateView();

            _craftButton.onClick.AddListener(OnCraftButtonClick);
            _playerProvider.Player.OnExperienceChanged += OnPlayerExperienceChanged;
        }

        private void Update()
        {
            if (_canCraft)
            {
                _oscTimer += Time.deltaTime * _buttonOscSpeed;

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
            var abilityData = _dataService.GetRandomCraftAbilityData(_levelFlowService.CurrentLevelIndex);
            _detailsZoneHud.CreateDetail(abilityData);
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
    }
}