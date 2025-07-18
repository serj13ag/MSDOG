using Core;

namespace Services.Gameplay
{
    public class GameStateService
    {
        private readonly int _levelIndex;

        private readonly EnemyService _enemyService;
        private readonly WindowService _windowService;
        private ProgressService _progressService;

        private Player _player;

        public int CurrentLevelIndex => _levelIndex;

        public GameStateService(int levelIndex, EnemyService enemyService, WindowService windowService,
            ProgressService progressService)
        {
            _levelIndex = levelIndex;

            _progressService = progressService;
            _enemyService = enemyService;
            _windowService = windowService;

            _enemyService.OnAllEnemiesDied += OnAllEnemiesDied;
        }

        public void RegisterPlayer(Player player)
        {
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