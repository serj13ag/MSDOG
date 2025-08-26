using Gameplay.Interfaces;

namespace Gameplay.Controllers
{
    public interface IGameplayUpdateController
    {
        void Register(IUpdatable updatable);
        void Remove(IUpdatable updatable);
    }
}