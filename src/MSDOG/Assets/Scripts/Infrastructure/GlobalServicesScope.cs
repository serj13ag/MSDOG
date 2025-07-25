using Infrastructure.StateMachine;
using Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure
{
    public class GlobalServicesScope : LifetimeScope
    {
        [SerializeField] private LoadingCurtainService _loadingCurtainService;
        [SerializeField] private SoundService _soundService;

        private GameStateMachine _gameStateMachine;

        protected override void Awake()
        {
            // Skip container building
        }

        public void SetGameStateMachine(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public void BuildContainer()
        {
            Build();
        }

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_gameStateMachine);

            var coroutineService = new GameObject("CoroutineService").AddComponent<CoroutineService>();
            builder.RegisterInstance(coroutineService);

            var updateService = new GameObject("UpdateService").AddComponent<UpdateService>();
            builder.RegisterInstance(updateService);

            builder.Register<AssetProviderService>(Lifetime.Singleton);
            builder.Register<DataService>(Lifetime.Singleton);
            builder.Register<ProgressService>(Lifetime.Singleton);
            builder.Register<SceneLoadService>(Lifetime.Singleton);
            builder.Register<WindowService>(Lifetime.Singleton);

            builder.RegisterInstance(_loadingCurtainService);
            builder.RegisterInstance(_soundService);
        }
    }
}