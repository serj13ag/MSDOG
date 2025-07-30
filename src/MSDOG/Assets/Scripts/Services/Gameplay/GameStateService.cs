using System;
using Core;
using Sounds;

namespace Services.Gameplay
{
    public class GameStateService : IDisposable
    {
        private readonly EnemyService _enemyService;
        private readonly WindowService _windowService;
        private readonly ProgressService _progressService;
        private readonly DialogueService _dialogueService;
        private readonly SoundService _soundService;

        private int _levelIndex;
        private bool _isLastLevel;
        private Player _player;

        public int CurrentLevelIndex => _levelIndex;
        public bool IsLastLevel => _isLastLevel;

        public GameStateService(EnemyService enemyService, WindowService windowService, ProgressService progressService,
            DialogueService dialogueService, SoundService soundService)
        {
            _progressService = progressService;
            _dialogueService = dialogueService;
            _soundService = soundService;
            _enemyService = enemyService;
            _windowService = windowService;

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
                _soundService.PlaySfx(SfxType.Death);

                _windowService.ShowLoseWindow();
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
            _windowService.ShowWinWindow();
            _windowService.ShowCreditsWindow();
        }

        private void ShowWinWindow()
        {
            _windowService.ShowWinWindow();
        }

        public void Dispose()
        {
            _player.OnHealthChanged -= OnPlayerHealthChanged;
            _enemyService.OnAllEnemiesDied -= OnAllEnemiesDied;
        }
    }
}