using System;
using Core.Interfaces;

namespace Core.Controllers
{
    public interface IUpdateController
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