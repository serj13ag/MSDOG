using GameplayTvHud.Actions;
using GameplayTvHud.Actions.Fuse;
using GameplayTvHud.DetailsZone;
using GameplayTvHud.Factories;
using GameplayTvHud.Mediators;
using GameplayTvHud.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure
{
    public class GameplayTvHudLifetimeScope : BaseLifetimeScope
    {
        [SerializeField] private HealthBarHud _healthBarHud;
        [SerializeField] private ExperienceBarHud _experienceBarHud;

        [SerializeField] private DetailsZoneHud _detailsZoneHud;
        [SerializeField] private ActiveZoneHud _activeZoneHud;
        [SerializeField] private DestructZoneHud _destructZoneHud;
        [SerializeField] private FusionZoneHud _fusionZoneHud;

        [SerializeField] private FuseAction _fuseAction;
        [SerializeField] private NitroAction _nitroAction;
        [SerializeField] private ReloadAction _reloadAction;

        protected override void ConfigureContainer(IContainerBuilder builder)
        {
            builder.RegisterComponent(_healthBarHud);

            builder.RegisterComponent(_experienceBarHud);
            builder.RegisterComponentInHierarchy<CraftButtonHud>();

            builder.RegisterComponent(_detailsZoneHud);
            builder.RegisterComponent(_activeZoneHud);
            builder.RegisterComponent(_destructZoneHud);
            builder.RegisterComponent(_fusionZoneHud);

            builder.RegisterComponent(_fuseAction);
            builder.RegisterComponent(_nitroAction);
            builder.RegisterComponent(_reloadAction);

            builder.Register<IDetailViewFactory, DetailViewFactory>(Lifetime.Singleton);

            builder.Register<IDetailMediator, DetailMediator>(Lifetime.Singleton);
            builder.Register<IActionMediator, ActionMediator>(Lifetime.Singleton);

            builder.Register<GameplayTvHudInitializer>(Lifetime.Singleton);
        }
    }
}