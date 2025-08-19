using Core.Models.Data;
using Core.Services;
using Gameplay.Services;
using UI.HUD.DetailsZone;
using UnityEngine;
using VContainer;

namespace UI.HUD
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private Transform _canvasTransform;

        private IDataService _dataService;
        private ILevelFlowService _levelFlowService;

        private DetailsZoneHud _detailsZoneHud;
        private ActiveZoneHud _activeZoneHud;

        [Inject]
        public void Construct(IDataService dataService, ILevelFlowService levelFlowService, DetailsZoneHud detailsZoneHud,
            ActiveZoneHud activeZoneHud)
        {
            _levelFlowService = levelFlowService;
            _dataService = dataService;

            _detailsZoneHud = detailsZoneHud;
            _activeZoneHud = activeZoneHud;
        }

        public void AddStartAbility()
        {
            var startAbilitiesData = _dataService.GetStartAbilitiesData(_levelFlowService.CurrentLevelIndex);
            foreach (var abilityData in startAbilitiesData)
            {
                _activeZoneHud.AddDetail(abilityData);
            }
        }

        public void AddAbility(AbilityData abilityData)
        {
            _detailsZoneHud.CreateDetail(abilityData);
        }
    }
}