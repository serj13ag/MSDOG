using Core.Interfaces;

namespace Core.Controllers
{
    public interface IUpdateController
    {
        bool IsPaused { get; }

        void Register(IUpdatable updatable);
        void Remove(IUpdatable updatable);

        void Pause(bool withTimeScale = false);
        void Unpause(bool withTimeScale = false);
    }
}