using Core;

namespace Services.Gameplay
{
    public class GameStateService
    {
        private readonly EnemyService _enemyService;
        private readonly WindowService _windowService;
        private readonly ProgressService _progressService;

        private int _levelIndex;
        private Player _player;

        public int CurrentLevelIndex => _levelIndex;

        public GameStateService(EnemyService enemyService, WindowService windowService, ProgressService progressService)
        {
            _progressService = progressService;
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
                _windowService.CreateLoseWindow();
            }
        }

        private void OnAllEnemiesDied()
        {
            _progressService.SetLastPassedLevel(_levelIndex);
            _windowService.CreateWinWindow();
        }

        public void Cleanup()
        {
            _player.OnHealthChanged -= OnPlayerHealthChanged;
            _enemyService.OnAllEnemiesDied -= OnAllEnemiesDied;
        }
    }
}