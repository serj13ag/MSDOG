using System;
using Gameplay.Interfaces;

namespace Gameplay.Controllers
{
    public interface IGameplayUpdateController
    {
        bool IsPaused { get; }
        float GameTimeScale { get; }

        event EventHandler<EventArgs> OnGameTimeChanged;

        void Register(IUpdatable updatable);
        void Remove(IUpdatable updatable);

        void Pause();
        void Unpause();
    }
}