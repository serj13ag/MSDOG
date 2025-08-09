using System;

namespace Gameplay.Providers
{
    public interface IPlayerProvider
    {
        Player Player { get; }

        event EventHandler<EventArgs> OnPlayerDied;

        void RegisterPlayer(Player player);
    }
}