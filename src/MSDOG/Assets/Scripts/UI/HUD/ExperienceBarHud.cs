using Services;
using Services.Gameplay;
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

        private DataService _dataService;
        private GameFactory _gameFactory;
        private GameStateService _gameStateService;

        private DetailsZoneHud _detailsZoneHud;

        [Inject]
        public void Construct(GameFactory gameFactory, DataService dataService, GameStateService gameStateService)
        {
            _gameStateService = gameStateService;
            _gameFactory = gameFactory;
            _dataService = dataService;
        }

        public void Init(DetailsZoneHud detailsZoneHud)
        {
            // TODO: refactor
            _detailsZoneHud = detailsZoneHud;

            UpdateView();

            _craftButton.onClick.AddListener(OnCraftButtonClick);
            _gameFactory.Player.OnExperienceChanged += OnPlayerExperienceChanged;
        }

        private void OnCraftButtonClick()
        {
            var abilityData = _dataService.GetRandomCraftAbilityData(_gameStateService.CurrentLevelIndex);
            _detailsZoneHud.CreateDetail(abilityData);
            _gameFactory.Player.ResetExperience();
        }

        private void OnPlayerExperienceChanged()
        {
            UpdateView();
        }

        private void UpdateView()
        {
            var player = _gameFactory.Player;

            _text.text = $"{player.CurrentExperience}/{player.MaxExperience}";
            _fillImage.fillAmount = (float)player.CurrentExperience / player.MaxExperience;

            _craftButton.interactable = player.CurrentExperience >= player.MaxExperience;
        }

        private void OnDestroy()
        {
            _craftButton.onClick.RemoveListener(OnCraftButtonClick);
            _gameFactory.Player.OnExperienceChanged -= OnPlayerExperienceChanged;
        }
    }
}