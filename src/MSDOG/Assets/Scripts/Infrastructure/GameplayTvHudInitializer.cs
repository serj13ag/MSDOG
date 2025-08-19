using Core.Services;
using Gameplay.Controllers;
using Gameplay.Providers;
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
        private readonly HudController _hudController;
        private readonly HealthBarHud _healthBarHud;
        private readonly ExperienceBarHud _experienceBarHud;
        private readonly ActiveZoneHud _activeZoneHud;
        private readonly HudActions _hudActions;
        private readonly IEscapeWindowHandler _escapeWindowHandler;
        private readonly IPlayerProvider _playerProvider;
        private readonly IDebugController _debugController;

        public GameplayTvHudInitializer(IDataService dataService,
            ILevelFlowService levelFlowService,
            HudController hudController,
            HealthBarHud healthBarHud,
            ExperienceBarHud experienceBarHud,
            ActiveZoneHud activeZoneHud,
            HudActions hudActions,
            IEscapeWindowHandler escapeWindowHandler,
            IPlayerProvider playerProvider,
            IDebugController debugController)
        {
            _dataService = dataService;
            _levelFlowService = levelFlowService;
            _hudController = hudController;
            _healthBarHud = healthBarHud;
            _experienceBarHud = experienceBarHud;
            _activeZoneHud = activeZoneHud;
            _hudActions = hudActions;
            _escapeWindowHandler = escapeWindowHandler;
            _playerProvider = playerProvider;
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

            _hudActions.Init(_playerProvider.Player);

            _debugController.Setup(_hudController);
        }
    }
}