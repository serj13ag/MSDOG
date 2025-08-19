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
        private readonly HudActions _hudActions;
        private readonly IPlayerProvider _playerProvider;
        private readonly IDebugController _debugController;

        public GameplayTvHudInitializer(HudController hudController,
            HealthBarHud healthBarHud,
            HudActions hudActions,
            IPlayerProvider playerProvider,
            IDebugController debugController)
        {
            _hudController = hudController;
            _healthBarHud = healthBarHud;
            _hudActions = hudActions;
            _playerProvider = playerProvider;
            _debugController = debugController;
        }

        public void Start()
        {
            _healthBarHud.Init();

            // TODO: refactor if needed
            _hudController.Init();
            _hudController.AddStartAbility();

            _hudActions.Init(_playerProvider.Player);

            _debugController.Setup(_hudController);
        }
    }
}