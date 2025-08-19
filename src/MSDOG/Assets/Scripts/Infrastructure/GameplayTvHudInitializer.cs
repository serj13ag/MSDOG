using Core.Services;
using Gameplay.Controllers;
using Gameplay.Services;
using UI.HUD;
using UI.HUD.Actions;
using UI.HUD.DetailsZone;

namespace Infrastructure
{
    public class GameplayTvHudInitializer
    {
        private readonly IDataService _dataService;
        private readonly ILevelFlowService _levelFlowService;
        private readonly HealthBarHud _healthBarHud;
        private readonly ExperienceBarHud _experienceBarHud;
        private readonly ActiveZoneHud _activeZoneHud;
        private readonly DetailsZoneHud _detailsZoneHud;
        private readonly FuseAction _fuseAction;
        private readonly NitroAction _nitroAction;
        private readonly ReloadAction _reloadAction;
        private readonly IEscapeWindowHandler _escapeWindowHandler;
        private readonly IDebugController _debugController;

        public GameplayTvHudInitializer(IDataService dataService,
            ILevelFlowService levelFlowService,
            HealthBarHud healthBarHud,
            ExperienceBarHud experienceBarHud,
            ActiveZoneHud activeZoneHud,
            DetailsZoneHud detailsZoneHud,
            FuseAction fuseAction,
            NitroAction nitroAction,
            ReloadAction reloadAction,
            IEscapeWindowHandler escapeWindowHandler,
            IDebugController debugController)
        {
            _dataService = dataService;
            _levelFlowService = levelFlowService;
            _healthBarHud = healthBarHud;
            _experienceBarHud = experienceBarHud;
            _activeZoneHud = activeZoneHud;
            _detailsZoneHud = detailsZoneHud;
            _fuseAction = fuseAction;
            _nitroAction = nitroAction;
            _reloadAction = reloadAction;
            _escapeWindowHandler = escapeWindowHandler;
            _debugController = debugController;
        }

        public void Start()
        {
            _healthBarHud.Init();
            _experienceBarHud.Init();

            _escapeWindowHandler.Init();

            var startAbilitiesData = _dataService.GetStartAbilitiesData(_levelFlowService.CurrentLevelIndex);
            foreach (var abilityData in startAbilitiesData)
            {
                _activeZoneHud.AddDetail(abilityData);
            }

            _fuseAction.Init();
            _nitroAction.Init();
            _reloadAction.Init();

            _debugController.Setup(_detailsZoneHud);
        }
    }
}