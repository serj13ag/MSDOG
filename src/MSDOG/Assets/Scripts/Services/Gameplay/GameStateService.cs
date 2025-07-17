using Core;

namespace Services.Gameplay
{
    public class GameStateService
    {
        private readonly EnemyService _enemyService;
        private readonly WindowService _windowService;

        private Player _player;

        public GameStateService(EnemyService enemyService, WindowService windowService)
        {
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
            _windowService.CreateWinWindow();
        }

        public void Cleanup()
        {
            _player.OnHealthChanged -= OnPlayerHealthChanged;
            _enemyService.OnAllEnemiesDied -= OnAllEnemiesDied;
        }
    }
}