using Gameplay.Controllers;
using Gameplay.Providers;
using UI.HUD;
using UI.HUD.Actions;

namespace Infrastructure
{
    public class GameplayTvHudInitializer
    {
        private readonly HudController _hudController;
        private readonly HealthBarHud _healthBarHud;
        private readonly ExperienceBarHud _experienceBarHud;
        private readonly HudActions _hudActions;
        private readonly IEscapeWindowHandler _escapeWindowHandler;
        private readonly IPlayerProvider _playerProvider;
        private readonly IDebugController _debugController;

        public GameplayTvHudInitializer(HudController hudController,
            HealthBarHud healthBarHud,
            ExperienceBarHud experienceBarHud,
            HudActions hudActions,
            IEscapeWindowHandler escapeWindowHandler,
            IPlayerProvider playerProvider,
            IDebugController debugController)
        {
            _hudController = hudController;
            _healthBarHud = healthBarHud;
            _experienceBarHud = experienceBarHud;
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

            // TODO: refactor if needed
            _hudController.AddStartAbility();

            _hudActions.Init(_playerProvider.Player);

            _debugController.Setup(_hudController);
        }
    }
}