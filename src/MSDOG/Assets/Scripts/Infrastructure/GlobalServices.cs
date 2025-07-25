using Infrastructure.StateMachine;
using Services;
using VContainer;

namespace Infrastructure
{
    public static class GlobalServices
    {
        private static IObjectResolver _container;

        public static GameStateMachine GameStateMachine => _container.Resolve<GameStateMachine>();

        public static AssetProviderService AssetProviderService => _container.Resolve<AssetProviderService>();
        public static DataService DataService => _container.Resolve<DataService>();
        public static LoadingCurtainService LoadingCurtainService => _container.Resolve<LoadingCurtainService>();
        public static SceneLoadService SceneLoadService => _container.Resolve<SceneLoadService>();
        public static SoundService SoundService => _container.Resolve<SoundService>();
        public static UpdateService UpdateService => _container.Resolve<UpdateService>();
        public static WindowService WindowService => _container.Resolve<WindowService>();
        public static ProgressService ProgressService => _container.Resolve<ProgressService>();

        public static void Initialize(IObjectResolver container)
        {
            _container = container;
        }
    }
}