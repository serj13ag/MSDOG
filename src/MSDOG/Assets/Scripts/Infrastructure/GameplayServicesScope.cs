using Gameplay.Services;
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

            builder.RegisterComponentOnNewGameObject<DebugService>(Lifetime.Scoped, "DebugService");

            RegisterContainers(builder);

            builder.Register<InputService>(Lifetime.Scoped);
            builder.Register<ArenaService>(Lifetime.Scoped);
            builder.Register<ProjectileFactory>(Lifetime.Scoped);
            builder.Register<AbilityFactory>(Lifetime.Scoped);
            builder.Register<GameFactory>(Lifetime.Scoped);
            builder.Register<EnemyService>(Lifetime.Scoped);
            builder.Register<GameStateService>(Lifetime.Scoped);
            builder.Register<GameplayWindowFactory>(Lifetime.Scoped);
            builder.Register<VfxFactory>(Lifetime.Scoped);
            builder.Register<TutorialService>(Lifetime.Scoped);

            builder.Register<GameplayInitializer>(Lifetime.Scoped);
        }

        private static void RegisterContainers(IContainerBuilder builder)
        {
            var projectileContainer = new GameObject("ProjectileContainer");
            var enemyContainer = new GameObject("EnemyContainer");
            builder.Register(_ => new RuntimeContainers(projectileContainer.transform, enemyContainer.transform),
                Lifetime.Scoped);
        }
    }
}