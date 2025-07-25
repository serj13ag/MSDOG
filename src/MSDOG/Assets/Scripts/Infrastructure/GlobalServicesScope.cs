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

        private GameStateMachine _gameStateMachine;

        protected override void Awake()
        {
            // Skip container building
            DontDestroyOnLoad(gameObject);
        }

        public void SetGameStateMachine(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        protected override void ConfigureContainer(IContainerBuilder builder)
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

            builder.RegisterInstance(_loadingCurtainService);
            builder.RegisterInstance(_soundService);
        }
    }
}