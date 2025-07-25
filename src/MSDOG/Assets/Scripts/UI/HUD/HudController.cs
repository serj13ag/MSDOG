using Core;
using Data;
using Services;
using UI.HUD.DetailsZone;
using UnityEngine;

namespace UI.HUD
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private HealthBarHud _healthBarHud;
        [SerializeField] private ExperienceBarHud _experienceBarHud;
        [SerializeField] private DetailsZoneHud _detailsZoneHud;
        [SerializeField] private ActiveZoneHud _activeZoneHud;

        private DataService _dataService;

        public void Init(Player player, DataService dataService, AssetProviderService assetProviderService,
            SoundService soundService)
        {
            _dataService = dataService;

            _healthBarHud.Init(player);
            _experienceBarHud.Init(player, dataService, _detailsZoneHud);
            _detailsZoneHud.Init(assetProviderService);
            _activeZoneHud.Init(player, assetProviderService, soundService);
        }

        public void AddStartAbility()
        {
            var startAbilityData = _dataService.GetStartAbilityData();
            _activeZoneHud.AddDetail(startAbilityData);
        }

        public void AddAbility(AbilityData abilityData)
        {
            _detailsZoneHud.CreateDetail(abilityData);
        }
    }
}