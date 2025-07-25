using Constants;
using Services;
using Services.Gameplay;
using UnityEngine;
using VContainer;

namespace Infrastructure
{
    public class GameplayServicesScope : BaseServicesScope
    {
        protected override void ConfigureContainer(IContainerBuilder builder)
        {
            builder.Register<CameraService>(resolver =>
            {
                var assetProvider = resolver.Resolve<AssetProviderService>();
                var cameraService = assetProvider.Instantiate<CameraService>(AssetPaths.CameraServicePath);
                var updateService = resolver.Resolve<UpdateService>();
                cameraService.Init(updateService);
                return cameraService;
            }, Lifetime.Scoped);

            builder.Register<InputService>(Lifetime.Scoped);
            builder.Register<ArenaService>(Lifetime.Scoped);
            builder.Register<ProjectileFactory>(Lifetime.Scoped);
            builder.Register<AbilityFactory>(Lifetime.Scoped);
            builder.Register<GameFactory>(Lifetime.Scoped);
            builder.Register<EnemyService>(Lifetime.Scoped);
            builder.Register<GameStateService>(Lifetime.Scoped);
            builder.Register<GameplayWindowService>(Lifetime.Scoped);

            builder.Register<DebugService>(resolver =>
            {
                var debugService = new GameObject("DebugService").AddComponent<DebugService>();
                var updateService = resolver.Resolve<UpdateService>();
                debugService.Init(updateService);
                return debugService;
            }, Lifetime.Scoped);

            builder.Register<GameplayInitializer>(Lifetime.Scoped);
        }
    }
}