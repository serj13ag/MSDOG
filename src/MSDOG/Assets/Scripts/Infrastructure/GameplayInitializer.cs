using Services;
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
        private readonly DialogueService _dialogueService;
        private readonly LevelViewService _levelViewService;
        private readonly SoundService _soundService;
        private readonly DataService _dataService;
        private readonly TutorialService _tutorialService;

        public GameplayInitializer(DebugService debugService, EnemyService enemyService, GameFactory gameFactory,
            CameraService cameraService, GameStateService gameStateService, DialogueService dialogueService,
            LevelViewService levelViewService, SoundService soundService, DataService dataService,
            TutorialService tutorialService)
        {
            _levelViewService = levelViewService;
            _soundService = soundService;
            _dataService = dataService;
            _tutorialService = tutorialService;
            _dialogueService = dialogueService;
            _debugService = debugService;
            _enemyService = enemyService;
            _gameFactory = gameFactory;
            _cameraService = cameraService;
            _gameStateService = gameStateService;
        }

        public void Start(int levelIndex)
        {
            var player = _gameFactory.CreatePlayer();

            var isLastLevel = _dataService.GetNumberOfLevels() == levelIndex + 1;
            _gameStateService.RegisterPlayer(player, levelIndex, isLastLevel);
            _cameraService.SetFollowTarget(player.transform);
            _levelViewService.InitializeLevel(levelIndex);
            _enemyService.SetupLevel(levelIndex, player.transform);

            var hudController = Object.FindFirstObjectByType<HudController>();
            hudController.Init();
            hudController.AddStartAbility();

            var hudActions = Object.FindFirstObjectByType<HudActions>();
            hudActions.Init(player);

            var levelMusic = _dataService.GetLevelData(levelIndex).Music;
            _soundService.PlayMusic(levelMusic);

            _debugService.Setup(hudController);
            _tutorialService.SetPlayer(player);

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