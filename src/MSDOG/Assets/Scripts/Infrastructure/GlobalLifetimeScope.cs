using Core.Controllers;
using Core.Factories;
using Core.Services;
using Infrastructure.StateMachine;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure
{
    public class GlobalLifetimeScope : BaseLifetimeScope
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
            RegisterControllers(builder);
            RegisterGameStateMachine(builder);
            RegisterServices(builder);
            RegisterFactories(builder);
        }

        private void RegisterControllers(IContainerBuilder builder)
        {
            builder.RegisterComponentOnNewGameObject<CoroutineController>(Lifetime.Singleton, "CoroutineController")
                .As<ICoroutineController>();

            builder.RegisterComponent(_loadingCurtainController).As<ILoadingCurtainController>();
            builder.RegisterComponent(_soundController).As<ISoundController>();
            builder.RegisterComponent(_windowController).As<IWindowController>();
        }

        private static void RegisterGameStateMachine(IContainerBuilder builder)
        {
            builder.Register<BootstrapState>(Lifetime.Singleton);
            builder.Register<MainMenuState>(Lifetime.Singleton);
            builder.Register<GameplayState>(Lifetime.Singleton);

            builder.Register<IGameStateMachine, GameStateMachine>(Lifetime.Singleton);
        }

        private static void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<IDataService, DataService>(Lifetime.Singleton);
            builder.Register<IProgressService, ProgressService>(Lifetime.Singleton);
            builder.Register<IPlayerOptionsService, PlayerOptionsService>(Lifetime.Singleton);
            builder.Register<ISceneLoadService, SceneLoadService>(Lifetime.Singleton);
            builder.Register<IDialogueService, DialogueService>(Lifetime.Singleton);
            builder.Register<ISaveLoadService, SaveLoadService>(Lifetime.Singleton);
        }

        private static void RegisterFactories(IContainerBuilder builder)
        {
            builder.Register<IGlobalWindowFactory, GlobalWindowFactory>(Lifetime.Singleton);
        }
    }
}