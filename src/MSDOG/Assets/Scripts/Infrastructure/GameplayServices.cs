using Services.Gameplay;
using VContainer;

namespace Infrastructure
{
    public static class GameplayServices
    {
        private static IObjectResolver _container;

        public static InputService InputService => _container.Resolve<InputService>();
        public static GameStateService GameStateService => _container.Resolve<GameStateService>();

        public static void Initialize(IObjectResolver container)
        {
            _container = container;
        }
    }
}