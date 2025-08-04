using Core.Controllers;
using Core.Factories;
using Core.Services;
using Infrastructure.StateMachine;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure
{
    public class GlobalServicesScope : BaseServicesScope
    {
        [SerializeField] private LoadingCurtainController _loadingCurtainController;
        [SerializeField] private SoundController _soundController;
        [SerializeField] private WindowController _windowController;

        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(gameObject);
        }

        protected override void ConfigureContainer(IContainerBuilder builder)
        {
            builder.RegisterComponentOnNewGameObject<CoroutineController>(Lifetime.Singleton, "CoroutineService");
            builder.RegisterComponentOnNewGameObject<UpdateController>(Lifetime.Singleton, "UpdateService");

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

            builder.RegisterComponent(_loadingCurtainController);
            builder.RegisterComponent(_soundController);
            builder.RegisterComponent(_windowController);
        }
    }
}