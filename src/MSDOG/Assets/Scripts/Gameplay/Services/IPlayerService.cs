using System;

namespace Gameplay.Services
{
    public interface IPlayerService
    {
        Player Player { get; }

        event EventHandler<EventArgs> OnPlayerDied;

        void RegisterPlayer(Player player);
    }
}