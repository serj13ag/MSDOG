using System;
using Core.Controllers;
using Core.Models.Data;
using Core.Services;
using Gameplay.Services;
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

        private IDataService _dataService;
        private WindowController _windowController;
        private InputService _inputService;
        private LevelFlowService _levelFlowService;

        [Inject]
        public void Construct(IDataService dataService, WindowController windowController, InputService inputService,
            LevelFlowService levelFlowService)
        {
            _levelFlowService = levelFlowService;
            _inputService = inputService;
            _windowController = windowController;
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

        private void OnMenuActionPerformed(object sender, EventArgs e)
        {
            if (_windowController.WindowIsActive<EscapeWindow>())
            {
                _windowController.CloseActiveWindow();
            }
            else
            {
                _windowController.ShowEscapeWindow();
            }
        }

        private void OnDestroy()
        {
            _inputService.OnMenuActionPerformed += OnMenuActionPerformed;
        }
    }
}