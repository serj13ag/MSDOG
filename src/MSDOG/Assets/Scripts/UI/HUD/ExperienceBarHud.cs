using Core;
using Services;
using TMPro;
using UI.HUD.DetailsZone;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    public class ExperienceBarHud : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _fillImage;
        [SerializeField] private Button _craftButton;

        private Player _player;
        private DataService _dataService;
        private DetailsZoneHud _detailsZoneHud;

        public void Init(Player player, DataService dataService, DetailsZoneHud detailsZoneHud)
        {
            _player = player;
            _dataService = dataService;
            _detailsZoneHud = detailsZoneHud;

            UpdateView();

            _craftButton.onClick.AddListener(OnCraftButtonClick);
            player.OnExperienceChanged += OnPlayerExperienceChanged;
        }

        private void OnCraftButtonClick()
        {
            var abilityData = _dataService.GetRandomCraftAbilityData();
            _detailsZoneHud.CreateDetail(abilityData);
            _player.ResetExperience();
        }

        private void OnPlayerExperienceChanged()
        {
            UpdateView();
        }

        private void UpdateView()
        {
            _text.text = $"{_player.CurrentExperience}/{_player.MaxExperience}";
            _fillImage.fillAmount = (float)_player.CurrentExperience / _player.MaxExperience;

            _craftButton.interactable = _player.CurrentExperience >= _player.MaxExperience;
        }

        private void OnDestroy()
        {
            _craftButton.onClick.RemoveListener(OnCraftButtonClick);
            _player.OnExperienceChanged -= OnPlayerExperienceChanged;
        }
    }
}