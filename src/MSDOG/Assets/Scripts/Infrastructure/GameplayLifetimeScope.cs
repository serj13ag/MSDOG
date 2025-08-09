using Gameplay.Controllers;
using Gameplay.Factories;
using Gameplay.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure
{
    public class GameplayLifetimeScope : BaseLifetimeScope
    {
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private LevelViewController _levelViewController;

        protected override void ConfigureContainer(IContainerBuilder builder)
        {
            builder.RegisterComponent(_cameraController).As<ICameraController>();
            builder.RegisterComponent(_levelViewController).As<ILevelViewController>();

            builder.RegisterComponentOnNewGameObject<DebugController>(Lifetime.Scoped, "DebugController")
                .As<IDebugController>();

            RegisterObjectContainerProvider(builder);

            builder.Register<InputService>(Lifetime.Scoped);
            builder.Register<ArenaService>(Lifetime.Scoped);
            builder.Register<EnemyService>(Lifetime.Scoped);
            builder.Register<PlayerService>(Lifetime.Scoped);
            builder.Register<LevelFlowService>(Lifetime.Scoped);
            builder.Register<TutorialService>(Lifetime.Scoped);

            builder.Register<ProjectileFactory>(Lifetime.Scoped);
            builder.Register<AbilityFactory>(Lifetime.Scoped);
            builder.Register<GameFactory>(Lifetime.Scoped);
            builder.Register<DeathKitFactory>(Lifetime.Scoped);
            builder.Register<GameplayWindowFactory>(Lifetime.Scoped);
            builder.Register<VfxFactory>(Lifetime.Scoped);

            builder.Register<GameplayInitializer>(Lifetime.Scoped);
        }

        private static void RegisterObjectContainerProvider(IContainerBuilder builder)
        {
            var projectileContainer = new GameObject("ProjectileContainer");
            var enemyContainer = new GameObject("EnemyContainer");
            var deathKitContainer = new GameObject("DeathKitContainer");
            builder.Register(_ => new ObjectContainerProvider(projectileContainer.transform,
                    enemyContainer.transform, deathKitContainer.transform),
                Lifetime.Scoped);
        }
    }
}