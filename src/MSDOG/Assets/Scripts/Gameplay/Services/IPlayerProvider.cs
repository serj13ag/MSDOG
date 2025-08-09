using System;

namespace Gameplay.Services
{
    public interface IPlayerProvider
    {
        Player Player { get; }

        event EventHandler<EventArgs> OnPlayerDied;

        void RegisterPlayer(Player player);
    }
}