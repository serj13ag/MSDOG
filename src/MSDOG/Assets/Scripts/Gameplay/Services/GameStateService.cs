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

        private int _levelIndex;
        private bool _isLastLevel;
        private Player _player;

        public int CurrentLevelIndex => _levelIndex;
        public bool IsLastLevel => _isLastLevel;

        public GameStateService(EnemyService enemyService, WindowController windowController, ProgressService progressService,
            DialogueService dialogueService, SoundController soundController)
        {
            _progressService = progressService;
            _dialogueService = dialogueService;
            _soundController = soundController;
            _enemyService = enemyService;
            _windowController = windowController;

            _enemyService.OnAllEnemiesDied += OnAllEnemiesDied;
        }

        public void RegisterPlayer(Player player, int levelIndex, bool isLastLevel)
        {
            _levelIndex = levelIndex;
            _isLastLevel = isLastLevel;
            _player = player;

            player.OnHealthChanged += OnPlayerHealthChanged;
        }

        private void OnPlayerHealthChanged()
        {
            if (_player.CurrentHealth <= 0)
            {
                _soundController.PlaySfx(SfxType.Death);

                _windowController.ShowLoseWindow();
            }
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
            _player.OnHealthChanged -= OnPlayerHealthChanged;
            _enemyService.OnAllEnemiesDied -= OnAllEnemiesDied;
        }
    }
}