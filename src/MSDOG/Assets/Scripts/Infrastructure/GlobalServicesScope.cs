using Infrastructure.StateMachine;
using Services;
using UnityEngine;
using VContainer;

namespace Infrastructure
{
    public class GlobalServicesScope : BaseServicesScope
    {
        [SerializeField] private LoadingCurtainService _loadingCurtainService;
        [SerializeField] private SoundService _soundService;

        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(gameObject);
        }

        protected override void ConfigureContainer(IContainerBuilder builder)
        {
            var coroutineService = new GameObject("CoroutineService").AddComponent<CoroutineService>();
            builder.RegisterInstance(coroutineService);

            var updateService = new GameObject("UpdateService").AddComponent<UpdateService>();
            builder.RegisterInstance(updateService);

            builder.Register<BootstrapState>(Lifetime.Singleton);
            builder.Register<MainMenuState>(Lifetime.Singleton);
            builder.Register<GameplayState>(Lifetime.Singleton);

            builder.Register<GameStateMachine>(Lifetime.Singleton);

            builder.Register<AssetProviderService>(Lifetime.Singleton);
            builder.Register<DataService>(Lifetime.Singleton);
            builder.Register<ProgressService>(Lifetime.Singleton);
            builder.Register<SceneLoadService>(Lifetime.Singleton);

            builder.RegisterInstance(_loadingCurtainService);
            builder.RegisterInstance(_soundService);
        }
    }
}