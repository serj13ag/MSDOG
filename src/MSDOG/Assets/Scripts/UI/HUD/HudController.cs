using System;
using Data;
using Services;
using Services.Gameplay;
using UI.HUD.DetailsZone;
using UnityEngine;
using VContainer;

namespace UI.HUD
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private Transform _canvasTransform;
        [SerializeField] private HealthBarHud _healthBarHud;
        [SerializeField] private ExperienceBarHud _experienceBarHud;
        [SerializeField] private DetailsZoneHud _detailsZoneHud;
        [SerializeField] private ActiveZoneHud _activeZoneHud;

        private DataService _dataService;
        private GameplayWindowService _gameplayWindowService;
        private InputService _inputService;

        private bool _escapeWindowIsActive;

        [Inject]
        public void Construct(DataService dataService, GameplayWindowService gameplayWindowService, InputService inputService)
        {
            _inputService = inputService;
            _gameplayWindowService = gameplayWindowService;
            _dataService = dataService;

            inputService.OnMenuActionPerformed += OnMenuActionPerformed;
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

        private void OnMenuActionPerformed(object sender, EventArgs e)
        {
            if (_escapeWindowIsActive)
            {
                _gameplayWindowService.CloseActiveWindow();
                _escapeWindowIsActive = false;
            }
            else
            {
                _gameplayWindowService.ShowEscape();
                _escapeWindowIsActive = true;
            }
        }

        private void OnDestroy()
        {
            _inputService.OnMenuActionPerformed += OnMenuActionPerformed;
        }
    }
}