using Services.Gameplay;
using UI.HUD;
using UI.HUD.Actions;
using UnityEngine;

namespace Infrastructure
{
    public class GameplayInitializer
    {
        private readonly DebugService _debugService;
        private readonly EnemyService _enemyService;
        private readonly GameFactory _gameFactory;
        private readonly CameraService _cameraService;
        private readonly GameStateService _gameStateService;

        public GameplayInitializer(DebugService debugService, EnemyService enemyService, GameFactory gameFactory,
            CameraService cameraService, GameStateService gameStateService)
        {
            _debugService = debugService;
            _enemyService = enemyService;
            _gameFactory = gameFactory;
            _cameraService = cameraService;
            _gameStateService = gameStateService;
        }

        public void Start(int levelIndex)
        {
            var player = _gameFactory.CreatePlayer();

            _gameStateService.RegisterPlayer(player, levelIndex);
            _cameraService.SetFollowTarget(player.transform);
            _enemyService.ActivateLevel(levelIndex, player.transform);

            var hudController = Object.FindFirstObjectByType<HudController>();
            hudController.Init();
            hudController.AddStartAbility();

            var hudActions = Object.FindFirstObjectByType<HudActions>();
            hudActions.Init(player);

            _debugService.Setup(hudController);
        }
    }
}