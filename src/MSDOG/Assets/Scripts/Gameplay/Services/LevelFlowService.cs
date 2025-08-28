using System;
using Core.Controllers;
using Core.Services;
using Core.Sounds;
using Gameplay.Providers;

namespace Gameplay.Services
{
    public class LevelFlowService : ILevelFlowService, IDisposable
    {
        private readonly IEnemyService _enemyService;
        private readonly IGameplayWindowService _gameplayWindowService;
        private readonly IProgressService _progressService;
        private readonly IDialogueService _dialogueService;
        private readonly ISoundController _soundController;
        private readonly IPlayerProvider _playerProvider;
        private readonly IDataService _dataService;

        private int _levelIndex;
        private bool _isLastLevel;
        private Player _player;

        public int CurrentLevelIndex => _levelIndex;
        public bool IsLastLevel => _isLastLevel;

        public LevelFlowService(IEnemyService enemyService, IGameplayWindowService gameplayWindowService,
            IProgressService progressService, IDialogueService dialogueService, ISoundController soundController,
            IPlayerProvider playerProvider, IDataService dataService)
        {
            _progressService = progressService;
            _dialogueService = dialogueService;
            _soundController = soundController;
            _playerProvider = playerProvider;
            _dataService = dataService;
            _enemyService = enemyService;
            _gameplayWindowService = gameplayWindowService;

            enemyService.OnAllEnemiesDied += OnAllEnemiesDied;
            playerProvider.OnPlayerDied += OnPlayerDied;
        }

        public void InitLevel(int levelIndex)
        {
            _levelIndex = levelIndex;
            _isLastLevel = _dataService.GetNumberOfLevels() == levelIndex + 1;
        }

        private void OnPlayerDied(object sender, EventArgs e)
        {
            _soundController.PlaySfx(SfxType.Death);
            _gameplayWindowService.ShowLoseWindow();
        }

        private void OnAllEnemiesDied()
        {
            _progressService.SetLastPassedLevel(_levelIndex);

            Action dialogueEndedAction = _isLastLevel? ShowCreditsAndWinWindow : ShowWinWindow;
            if (!_dialogueService.TryShowEndLevelDialogue(_levelIndex, dialogueEndedAction))
            {
                dialogueEndedAction.Invoke();
            }
        }

        private void ShowCreditsAndWinWindow()
        {
            _gameplayWindowService.ShowWinWindow();
            _gameplayWindowService.ShowCreditsWindow();
        }

        private void ShowWinWindow()
        {
            _gameplayWindowService.ShowWinWindow();
        }

        public void Dispose()
        {
            _enemyService.OnAllEnemiesDied -= OnAllEnemiesDied;
            _playerProvider.OnPlayerDied -= OnPlayerDied;
        }
    }
}