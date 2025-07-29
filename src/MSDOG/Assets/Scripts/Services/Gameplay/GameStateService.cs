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
        private Player _player;

        public int CurrentLevelIndex => _levelIndex;

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

        public void RegisterPlayer(Player player, int levelIndex)
        {
            _levelIndex = levelIndex;
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

            if (!_dialogueService.TryShowEndLevelDialogue(_levelIndex, ShowWinWindow))
            {
                ShowWinWindow();
            }
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