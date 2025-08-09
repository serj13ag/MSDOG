using Gameplay.Controllers;
using Gameplay.Factories;
using Gameplay.Providers;
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
            RegisterControllers(builder);
            RegisterObjectContainerProvider(builder);
            RegisterServices(builder);
            RegisterFactories(builder);

            builder.Register<GameplayInitializer>(Lifetime.Scoped);
        }

        private void RegisterControllers(IContainerBuilder builder)
        {
            builder.RegisterComponent(_cameraController).As<ICameraController>();
            builder.RegisterComponent(_levelViewController).As<ILevelViewController>();

            builder.RegisterComponentOnNewGameObject<DebugController>(Lifetime.Scoped, "DebugController")
                .As<IDebugController>();
        }

        private static void RegisterObjectContainerProvider(IContainerBuilder builder)
        {
            var containersRoot = new GameObject("ObjectContainers");

            var projectileContainer = new GameObject("ProjectileContainer");
            var enemyContainer = new GameObject("EnemyContainer");
            var deathKitContainer = new GameObject("DeathKitContainer");

            projectileContainer.transform.SetParent(containersRoot.transform);
            enemyContainer.transform.SetParent(containersRoot.transform);
            deathKitContainer.transform.SetParent(containersRoot.transform);

            builder.Register(_ => new ObjectContainerProvider(projectileContainer.transform,
                    enemyContainer.transform, deathKitContainer.transform),
                Lifetime.Scoped);
        }

        private static void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<IInputService, InputService>(Lifetime.Scoped);
            builder.Register<IArenaService, ArenaService>(Lifetime.Scoped);
            builder.Register<IEnemyService, EnemyService>(Lifetime.Scoped);
            builder.Register<IPlayerService, PlayerService>(Lifetime.Scoped);
            builder.Register<ILevelFlowService, LevelFlowService>(Lifetime.Scoped);
            builder.Register<ITutorialService, TutorialService>(Lifetime.Scoped);
        }

        private static void RegisterFactories(IContainerBuilder builder)
        {
            builder.Register<IProjectileFactory, ProjectileFactory>(Lifetime.Scoped);
            builder.Register<IAbilityFactory, AbilityFactory>(Lifetime.Scoped);
            builder.Register<IGameFactory, GameFactory>(Lifetime.Scoped);
            builder.Register<IDeathKitFactory, DeathKitFactory>(Lifetime.Scoped);
            builder.Register<IGameplayWindowFactory, GameplayWindowFactory>(Lifetime.Scoped);
            builder.Register<IVfxFactory, VfxFactory>(Lifetime.Scoped);
        }
    }
}