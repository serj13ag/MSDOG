using System;
using Core.Controllers;
using Core.Services;
using Core.Sounds;

namespace Gameplay.Services
{
    public class LevelFlowService : ILevelFlowService, IDisposable
    {
        private readonly IEnemyService _enemyService;
        private readonly IWindowController _windowController;
        private readonly IProgressService _progressService;
        private readonly IDialogueService _dialogueService;
        private readonly ISoundController _soundController;
        private readonly IPlayerService _playerService;
        private readonly IDataService _dataService;

        private int _levelIndex;
        private bool _isLastLevel;
        private Player _player;

        public int CurrentLevelIndex => _levelIndex;
        public bool IsLastLevel => _isLastLevel;

        public LevelFlowService(IEnemyService enemyService, IWindowController windowController, IProgressService progressService,
            IDialogueService dialogueService, ISoundController soundController, IPlayerService playerService,
            IDataService dataService)
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

            Action dialogueEndedAction = _isLastLevel? ShowCreditsAndWinWindow : ShowWinWindow;
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