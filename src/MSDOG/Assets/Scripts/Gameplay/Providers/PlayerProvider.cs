using System;

namespace Gameplay.Providers
{
    public class PlayerProvider : IPlayerProvider, IDisposable
    {
        private Player _player;

        public Player Player => _player;

        public event EventHandler<EventArgs> OnPlayerDied;

        public void RegisterPlayer(Player player)
        {
            _player = player;

            player.OnHealthChanged += OnPlayerHealthChanged;
        }

        private void OnPlayerHealthChanged()
        {
            if (_player.CurrentHealth <= 0)
            {
                OnPlayerDied?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Dispose()
        {
            _player.OnHealthChanged -= OnPlayerHealthChanged;
        }
    }
}