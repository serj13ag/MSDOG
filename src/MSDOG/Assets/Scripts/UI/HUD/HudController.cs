using Data;
using Services;
using UI.HUD.DetailsZone;
using UnityEngine;
using VContainer;

namespace UI.HUD
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private HealthBarHud _healthBarHud;
        [SerializeField] private ExperienceBarHud _experienceBarHud;
        [SerializeField] private DetailsZoneHud _detailsZoneHud;
        [SerializeField] private ActiveZoneHud _activeZoneHud;

        private DataService _dataService;

        [Inject]
        public void Construct(DataService dataService)
        {
            _dataService = dataService;
        }

        public void Init()
        {
            // TODO: refactor
            _healthBarHud.Init();
            _experienceBarHud.Init(_detailsZoneHud);
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