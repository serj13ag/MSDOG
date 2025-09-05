using System;

namespace Gameplay.Providers
{
    public interface IPlayerProvider
    {
        IPlayer Player { get; }

        event EventHandler<EventArgs> OnPlayerDied;

        void RegisterPlayer(IPlayer player);
    }
}