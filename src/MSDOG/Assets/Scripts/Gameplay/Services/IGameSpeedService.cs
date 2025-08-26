using System;

namespace Gameplay.Services
{
    public interface IGameSpeedService
    {
        float GameSpeed { get; }
        bool IsPaused { get; }

        event EventHandler<EventArgs> OnGameTimeChanged;

        void Pause();
        void Unpause();
    }
}