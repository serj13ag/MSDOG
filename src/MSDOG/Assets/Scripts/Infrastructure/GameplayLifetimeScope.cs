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
            RegisterProviders(builder);
            RegisterServices(builder);
            RegisterFactories(builder);

            builder.Register<GameplayInitializer>(Lifetime.Singleton);
        }

        private void RegisterControllers(IContainerBuilder builder)
        {
            builder.RegisterComponentOnNewGameObject<GameplayUpdateController>(Lifetime.Singleton, "GameplayUpdateController")
                .As<IGameplayUpdateController>();
            builder.RegisterComponent(_cameraController).As<ICameraController>();
            builder.RegisterComponent(_levelViewController).As<ILevelViewController>();

            builder.RegisterComponentOnNewGameObject<DebugController>(Lifetime.Singleton, "DebugController")
                .As<IDebugController>();
        }

        private static void RegisterProviders(IContainerBuilder builder)
        {
            RegisterObjectContainerProvider(builder);

            builder.Register<IPlayerProvider, PlayerProvider>(Lifetime.Singleton);
        }

        private static void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<IGameSpeedService, GameSpeedService>(Lifetime.Singleton);
            builder.Register<IInputService, InputService>(Lifetime.Singleton);
            builder.Register<IArenaService, ArenaService>(Lifetime.Singleton);
            builder.Register<IEnemyService, EnemyService>(Lifetime.Singleton);
            builder.Register<ILevelFlowService, LevelFlowService>(Lifetime.Singleton);
            builder.Register<ITutorialService, TutorialService>(Lifetime.Singleton);
            builder.Register<IDetailService, DetailService>(Lifetime.Singleton);

            builder.Register<IGameplayWindowsHandler, GameplayWindowsHandler>(Lifetime.Singleton);
        }

        private static void RegisterFactories(IContainerBuilder builder)
        {
            builder.Register<IExperiencePieceFactory, ExperiencePieceFactory>(Lifetime.Singleton);
            builder.Register<IProjectileFactory, ProjectileFactory>(Lifetime.Singleton);
            builder.Register<IAbilityEffectFactory, AbilityEffectFactory>(Lifetime.Singleton);
            builder.Register<IAbilityFactory, AbilityFactory>(Lifetime.Singleton);
            builder.Register<IGameFactory, GameFactory>(Lifetime.Singleton);
            builder.Register<IDeathKitFactory, DeathKitFactory>(Lifetime.Singleton);
            builder.Register<IGameplayWindowFactory, GameplayWindowFactory>(Lifetime.Singleton);
            builder.Register<IVfxFactory, VfxFactory>(Lifetime.Singleton);
            builder.Register<IDamageTextFactory, DamageTextFactory>(Lifetime.Singleton);
        }

        private static void RegisterObjectContainerProvider(IContainerBuilder builder)
        {
            var containersRoot = new GameObject("ObjectContainers");

            var projectileContainer = new GameObject("ProjectileContainer");
            var enemyContainer = new GameObject("EnemyContainer");
            var deathKitContainer = new GameObject("DeathKitContainer");
            var experiencePieceContainer = new GameObject("ExperiencePieceContainer");
            var damageTextContainer = new GameObject("DamageTextContainer");
            var abilityEffectContainer = new GameObject("AbilityEffectContainer");

            projectileContainer.transform.SetParent(containersRoot.transform);
            enemyContainer.transform.SetParent(containersRoot.transform);
            deathKitContainer.transform.SetParent(containersRoot.transform);
            experiencePieceContainer.transform.SetParent(containersRoot.transform);
            damageTextContainer.transform.SetParent(containersRoot.transform);
            abilityEffectContainer.transform.SetParent(containersRoot.transform);

            builder.Register(_ => new ObjectContainerProvider(projectileContainer.transform,
                    enemyContainer.transform, deathKitContainer.transform, experiencePieceContainer.transform,
                    damageTextContainer.transform, abilityEffectContainer.transform),
                Lifetime.Singleton).As<IObjectContainerProvider>();
        }
    }
}