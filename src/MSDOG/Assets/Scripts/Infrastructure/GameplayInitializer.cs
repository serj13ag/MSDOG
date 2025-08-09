using Core.Controllers;
using Core.Services;
using Gameplay.Controllers;
using Gameplay.Factories;
using Gameplay.Services;
using UI.HUD;
using UI.HUD.Actions;
using UnityEngine;

namespace Infrastructure
{
    public class GameplayInitializer
    {
        private readonly IDebugController _debugController;
        private readonly IEnemyService _enemyService;
        private readonly IGameFactory _gameFactory;
        private readonly ICameraController _cameraController;
        private readonly ILevelFlowService _levelFlowService;
        private readonly IDialogueService _dialogueService;
        private readonly ILevelViewController _levelViewController;
        private readonly ISoundController _soundController;
        private readonly IDataService _dataService;
        private readonly ITutorialService _tutorialService;
        private readonly IDeathKitFactory _deathKitFactory;
        private readonly IProjectileFactory _projectileFactory;
        private readonly IPlayerService _playerService;

        public GameplayInitializer(IDebugController debugController, IEnemyService enemyService, IGameFactory gameFactory,
            ICameraController cameraController, ILevelFlowService levelFlowService, IDialogueService dialogueService,
            ILevelViewController levelViewController, ISoundController soundController, IDataService dataService,
            ITutorialService tutorialService, IDeathKitFactory deathKitFactory, IProjectileFactory projectileFactory,
            IPlayerService playerService)
        {
            _levelViewController = levelViewController;
            _soundController = soundController;
            _dataService = dataService;
            _tutorialService = tutorialService;
            _deathKitFactory = deathKitFactory;
            _projectileFactory = projectileFactory;
            _playerService = playerService;
            _dialogueService = dialogueService;
            _debugController = debugController;
            _enemyService = enemyService;
            _gameFactory = gameFactory;
            _cameraController = cameraController;
            _levelFlowService = levelFlowService;
        }

        public void Start(int levelIndex)
        {
            _deathKitFactory.Prewarm(levelIndex);
            _projectileFactory.Prewarm(levelIndex);

            // TODO: refactor?
            var player = _gameFactory.CreatePlayer();
            _playerService.RegisterPlayer(player);
            _tutorialService.SetPlayer(player);
            _cameraController.SetFollowTarget(player.transform);

            _levelFlowService.InitLevel(levelIndex);
            _levelViewController.InitLevel(levelIndex);
            _enemyService.InitLevel(levelIndex, player.transform);

            var hudController = Object.FindFirstObjectByType<HudController>();
            hudController.Init();
            hudController.AddStartAbility();

            var hudActions = Object.FindFirstObjectByType<HudActions>();
            hudActions.Init(player);

            var levelMusic = _dataService.GetLevelData(levelIndex).Music;
            _soundController.PlayMusic(levelMusic);

            _debugController.Setup(hudController);

            if (!_dialogueService.TryShowStartLevelDialogue(levelIndex, ActivateLevel))
            {
                ActivateLevel();
            }
        }

        private void ActivateLevel()
        {
            _enemyService.ActivateLevel();
            _tutorialService.OnLevelActivated();
        }
    }
}