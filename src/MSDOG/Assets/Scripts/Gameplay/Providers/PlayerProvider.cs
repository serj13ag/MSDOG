using System;

namespace Gameplay.Providers
{
    public class PlayerProvider : IPlayerProvider, IDisposable
    {
        private IPlayer _player;

        public IPlayer Player => _player;

        public event EventHandler<EventArgs> OnPlayerDied;

        public void RegisterPlayer(IPlayer player)
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