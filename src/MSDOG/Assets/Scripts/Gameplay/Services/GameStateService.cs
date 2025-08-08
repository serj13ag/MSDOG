using System;
using Core.Controllers;
using Core.Services;
using Core.Sounds;

namespace Gameplay.Services
{
    public class GameStateService : IDisposable
    {
        private readonly EnemyService _enemyService;
        private readonly WindowController _windowController;
        private readonly ProgressService _progressService;
        private readonly DialogueService _dialogueService;
        private readonly SoundController _soundController;
        private readonly PlayerService _playerService;
        private readonly DataService _dataService;

        private int _levelIndex;
        private bool _isLastLevel;
        private Player _player;

        public int CurrentLevelIndex => _levelIndex;
        public bool IsLastLevel => _isLastLevel;

        public GameStateService(EnemyService enemyService, WindowController windowController, ProgressService progressService,
            DialogueService dialogueService, SoundController soundController, PlayerService playerService,
            DataService dataService)
        {
            _progressService = progressService;
            _dialogueService = dialogueService;
            _soundController = soundController;
            _playerService = playerService;
            _dataService = dataService;
            _enemyService = enemyService;
            _windowController = windowController;

            enemyService.OnAllEnemiesDied += OnAllEnemiesDied;
            playerService.OnPlayerDied += OnPlayerDied;
        }

        public void InitLevel(int levelIndex)
        {
            _levelIndex = levelIndex;
            _isLastLevel = _dataService.GetNumberOfLevels() == levelIndex + 1;
        }

        private void OnPlayerDied(object sender, EventArgs e)
        {
            _soundController.PlaySfx(SfxType.Death);
            _windowController.ShowLoseWindow();
        }

        private void OnAllEnemiesDied()
        {
            _progressService.SetLastPassedLevel(_levelIndex);

            Action dialogueEndedAction = _isLastLevel ? ShowCreditsAndWinWindow : ShowWinWindow;
            if (!_dialogueService.TryShowEndLevelDialogue(_levelIndex, dialogueEndedAction))
            {
                dialogueEndedAction.Invoke();
            }
        }

        private void ShowCreditsAndWinWindow()
        {
            _windowController.ShowWinWindow();
            _windowController.ShowCreditsWindow();
        }

        private void ShowWinWindow()
        {
            _windowController.ShowWinWindow();
        }

        public void Dispose()
        {
            _enemyService.OnAllEnemiesDied -= OnAllEnemiesDied;
            _playerService.OnPlayerDied -= OnPlayerDied;
        }
    }
}