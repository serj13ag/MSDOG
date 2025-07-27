using Services.Gameplay;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure
{
    public class GameplayServicesScope : BaseServicesScope
    {
        [SerializeField] private CameraService _cameraService;
        [SerializeField] private LevelViewService _levelViewService;

        protected override void ConfigureContainer(IContainerBuilder builder)
        {
            builder.RegisterComponent(_cameraService);
            builder.RegisterComponent(_levelViewService);

            builder.Register<InputService>(Lifetime.Scoped);
            builder.Register<ArenaService>(Lifetime.Scoped);
            builder.Register<ProjectileFactory>(Lifetime.Scoped);
            builder.Register<AbilityFactory>(Lifetime.Scoped);
            builder.Register<GameFactory>(Lifetime.Scoped);
            builder.Register<EnemyService>(Lifetime.Scoped);
            builder.Register<GameStateService>(Lifetime.Scoped);
            builder.Register<GameplayWindowFactory>(Lifetime.Scoped);
            builder.Register<VfxFactory>(Lifetime.Scoped);

            var debugService = new GameObject("DebugService").AddComponent<DebugService>();
            builder.RegisterComponent(debugService);

            builder.Register<GameplayInitializer>(Lifetime.Scoped);
        }
    }
}