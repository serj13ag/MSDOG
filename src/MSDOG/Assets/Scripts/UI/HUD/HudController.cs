using System;
using Data;
using Services;
using Services.Gameplay;
using UI.HUD.DetailsZone;
using UI.Windows;
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
        private WindowService _windowService;
        private InputService _inputService;
        private GameStateService _gameStateService;

        [Inject]
        public void Construct(DataService dataService, WindowService windowService, InputService inputService,
            GameStateService gameStateService)
        {
            _gameStateService = gameStateService;
            _inputService = inputService;
            _windowService = windowService;
            _dataService = dataService;

            inputService.OnMenuActionPerformed += OnMenuActionPerformed;
        }

        public void Init()
        {
            // TODO: refactor
            _healthBarHud.Init();
            _experienceBarHud.Init(_detailsZoneHud);
            _detailsZoneHud.Init(_activeZoneHud);
        }

        public void AddStartAbility()
        {
            var startAbilitiesData = _dataService.GetStartAbilitiesData(_gameStateService.CurrentLevelIndex);
            foreach (var abilityData in startAbilitiesData)
            {
                _activeZoneHud.AddDetail(abilityData);
            }
        }

        public void AddAbility(AbilityData abilityData)
        {
            _detailsZoneHud.CreateDetail(abilityData);
        }

        private void OnMenuActionPerformed(object sender, EventArgs e)
        {
            if (_windowService.WindowIsActive<EscapeWindow>())
            {
                _windowService.CloseActiveWindow();
            }
            else
            {
                _windowService.ShowEscapeWindow();
            }
        }

        private void OnDestroy()
        {
            _inputService.OnMenuActionPerformed += OnMenuActionPerformed;
        }
    }
}