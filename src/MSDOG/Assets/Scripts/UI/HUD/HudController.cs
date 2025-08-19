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

        private IDataService _dataService;
        private IWindowController _windowController;
        private IInputService _inputService;
        private ILevelFlowService _levelFlowService;

        private DetailsZoneHud _detailsZoneHud;
        private ActiveZoneHud _activeZoneHud;

        [Inject]
        public void Construct(IDataService dataService, IWindowController windowController, IInputService inputService,
            ILevelFlowService levelFlowService, DetailsZoneHud detailsZoneHud, ActiveZoneHud activeZoneHud)
        {
            _levelFlowService = levelFlowService;
            _inputService = inputService;
            _windowController = windowController;
            _dataService = dataService;

            _detailsZoneHud = detailsZoneHud;
            _activeZoneHud = activeZoneHud;

            inputService.OnMenuActionPerformed += OnMenuActionPerformed;
        }

        public void Init()
        {
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
            _inputService.OnMenuActionPerformed -= OnMenuActionPerformed;
        }
    }
}