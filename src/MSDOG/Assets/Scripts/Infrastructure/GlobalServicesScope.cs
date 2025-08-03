using Infrastructure.StateMachine;
using Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure
{
    public class GlobalServicesScope : BaseServicesScope
    {
        [SerializeField] private LoadingCurtainService _loadingCurtainService;
        [SerializeField] private SoundService _soundService;
        [SerializeField] private WindowService _windowService;

        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(gameObject);
        }

        protected override void ConfigureContainer(IContainerBuilder builder)
        {
            builder.RegisterComponentOnNewGameObject<CoroutineService>(Lifetime.Singleton, "CoroutineService");
            builder.RegisterComponentOnNewGameObject<UpdateService>(Lifetime.Singleton, "UpdateService");

            builder.Register<BootstrapState>(Lifetime.Singleton);
            builder.Register<MainMenuState>(Lifetime.Singleton);
            builder.Register<GameplayState>(Lifetime.Singleton);

            builder.Register<GameStateMachine>(Lifetime.Singleton);

            builder.Register<AssetProviderService>(Lifetime.Singleton);
            builder.Register<DataService>(Lifetime.Singleton);
            builder.Register<ProgressService>(Lifetime.Singleton);
            builder.Register<PlayerOptionsService>(Lifetime.Singleton);
            builder.Register<SceneLoadService>(Lifetime.Singleton);
            builder.Register<GlobalWindowFactory>(Lifetime.Singleton);
            builder.Register<DialogueService>(Lifetime.Singleton);
            builder.Register<SaveLoadService>(Lifetime.Singleton);

            builder.RegisterComponent(_loadingCurtainService);
            builder.RegisterComponent(_soundService);
            builder.RegisterComponent(_windowService);
        }
    }
}