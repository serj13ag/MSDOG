using Core.Controllers;
using Core.Services;
using Gameplay.Controllers;
using Gameplay.Factories;
using Gameplay.Providers;
using Gameplay.Services;

namespace Infrastructure
{
    public class GameplayInitializer
    {
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
        private readonly IPlayerProvider _playerProvider;
        private readonly IExperiencePieceFactory _experiencePieceFactory;
        private readonly IDamageTextFactory _damageTextFactory;
        private readonly IAbilityEffectFactory _abilityEffectFactory;
        private readonly IDetailService _detailService;
        private readonly IGameplayWindowsHandler _gameplayWindowsHandler;

        public GameplayInitializer(IEnemyService enemyService,
            IGameFactory gameFactory,
            ICameraController cameraController,
            ILevelFlowService levelFlowService,
            IDialogueService dialogueService,
            ILevelViewController levelViewController,
            ISoundController soundController,
            IDataService dataService,
            ITutorialService tutorialService,
            IDeathKitFactory deathKitFactory,
            IProjectileFactory projectileFactory,
            IPlayerProvider playerProvider,
            IExperiencePieceFactory experiencePieceFactory,
            IDamageTextFactory damageTextFactory,
            IAbilityEffectFactory abilityEffectFactory,
            IDetailService detailService,
            IGameplayWindowsHandler gameplayWindowsHandler)
        {
            _levelViewController = levelViewController;
            _soundController = soundController;
            _dataService = dataService;
            _tutorialService = tutorialService;
            _deathKitFactory = deathKitFactory;
            _projectileFactory = projectileFactory;
            _playerProvider = playerProvider;
            _experiencePieceFactory = experiencePieceFactory;
            _damageTextFactory = damageTextFactory;
            _abilityEffectFactory = abilityEffectFactory;
            _detailService = detailService;
            _gameplayWindowsHandler = gameplayWindowsHandler;
            _dialogueService = dialogueService;
            _enemyService = enemyService;
            _gameFactory = gameFactory;
            _cameraController = cameraController;
            _levelFlowService = levelFlowService;
        }

        public void Start(int levelIndex)
        {
            PrewarmFactories(levelIndex);
            SetupPlayer();
            CreateStartDetails(levelIndex);
            InitLevelSystems(levelIndex);

            _gameplayWindowsHandler.Init(); // TODO: need?

            PlayMusic(levelIndex);

            if (!_dialogueService.TryShowStartLevelDialogue(levelIndex, ActivateLevel))
            {
                ActivateLevel();
            }
        }

        private void PrewarmFactories(int levelIndex)
        {
            _deathKitFactory.Prewarm(levelIndex);
            _projectileFactory.Prewarm(levelIndex);
            _experiencePieceFactory.Prewarm();
            _damageTextFactory.Prewarm();
            _abilityEffectFactory.Prewarm(levelIndex);
        }

        private void SetupPlayer()
        {
            var player = _gameFactory.CreatePlayer();
            _playerProvider.RegisterPlayer(player);
            _tutorialService.StartTrackPlayer(player);
            _cameraController.SetFollowTarget(player.transform);
        }

        private void CreateStartDetails(int levelIndex)
        {
            var startAbilitiesData = _dataService.GetStartAbilitiesData(levelIndex);
            foreach (var abilityData in startAbilitiesData)
            {
                _detailService.CreateActiveDetail(abilityData);
            }
        }

        private void InitLevelSystems(int levelIndex)
        {
            _levelFlowService.InitLevel(levelIndex);
            _levelViewController.InitLevel(levelIndex);
            _enemyService.InitLevel(levelIndex);
        }

        private void PlayMusic(int levelIndex)
        {
            var levelMusic = _dataService.GetLevelData(levelIndex).Music;
            _soundController.PlayMusic(levelMusic);
        }

        private void ActivateLevel()
        {
            _enemyService.ActivateLevel();
            _tutorialService.OnLevelActivated();
        }
    }
}