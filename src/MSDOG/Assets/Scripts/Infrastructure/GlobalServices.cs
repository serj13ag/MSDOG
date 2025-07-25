using Infrastructure.StateMachine;
using Services;
using VContainer;

namespace Infrastructure
{
    public static class GlobalServices
    {
        private static IObjectResolver _container;

        public static GameStateMachine GameStateMachine => _container.Resolve<GameStateMachine>();

        public static LoadingCurtainService LoadingCurtainService => _container.Resolve<LoadingCurtainService>();
        public static SceneLoadService SceneLoadService => _container.Resolve<SceneLoadService>();

        public static void Initialize(IObjectResolver container)
        {
            _container = container;
        }
    }
}