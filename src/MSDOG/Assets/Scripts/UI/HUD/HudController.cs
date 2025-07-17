using Core;
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
        private AssetProviderService _assetProviderService;

        public void Init(Player player, DataService dataService, AssetProviderService assetProviderService)
        {
            _assetProviderService = assetProviderService;
            _dataService = dataService;

            _healthBarHud.Init(player);
            _experienceBarHud.Init(player, dataService, _detailsZoneHud);
            _detailsZoneHud.Init(assetProviderService);
            _activeZoneHud.Init(player, assetProviderService);
        }

        public void AddStartAbility()
        {
            var startAbilityData = _dataService.GetStartAbilityData();
            _activeZoneHud.AddDetail(startAbilityData);
        }
    }
}