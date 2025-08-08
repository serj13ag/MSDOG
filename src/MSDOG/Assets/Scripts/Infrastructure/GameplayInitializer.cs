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
        private readonly DebugController _debugController;
        private readonly EnemyService _enemyService;
        private readonly GameFactory _gameFactory;
        private readonly CameraController _cameraController;
        private readonly GameStateService _gameStateService;
        private readonly DialogueService _dialogueService;
        private readonly LevelViewController _levelViewController;
        private readonly SoundController _soundController;
        private readonly DataService _dataService;
        private readonly TutorialService _tutorialService;
        private readonly DeathKitFactory _deathKitFactory;
        private readonly ProjectileFactory _projectileFactory;
        private readonly PlayerService _playerService;

        public GameplayInitializer(DebugController debugController, EnemyService enemyService, GameFactory gameFactory,
            CameraController cameraController, GameStateService gameStateService, DialogueService dialogueService,
            LevelViewController levelViewController, SoundController soundController, DataService dataService,
            TutorialService tutorialService, DeathKitFactory deathKitFactory, ProjectileFactory projectileFactory,
            PlayerService playerService)
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
            _gameStateService = gameStateService;
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

            _gameStateService.InitLevel(levelIndex);
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