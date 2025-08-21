using GameplayView;
using GameplayView.DetailsZone;
using UI.HUD;
using UI.HUD.Actions;
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
            // TODO: register all child components and remove them from hud controller
            builder.RegisterComponent(_healthBarHud).As<HealthBarHud>();
            builder.RegisterComponent(_experienceBarHud).As<ExperienceBarHud>();

            builder.RegisterComponent(_detailsZoneHud).As<DetailsZoneHud>();
            builder.RegisterComponent(_activeZoneHud).As<ActiveZoneHud>();
            builder.RegisterComponent(_destructZoneHud).As<DestructZoneHud>();
            builder.RegisterComponent(_fusionZoneHud).As<FusionZoneHud>();

            builder.RegisterComponent(_fuseAction).As<FuseAction>();
            builder.RegisterComponent(_nitroAction).As<NitroAction>();
            builder.RegisterComponent(_reloadAction).As<ReloadAction>();

            builder.Register<IDetailMediator, DetailMediator>(Lifetime.Singleton);
            builder.Register<IActionMediator, ActionMediator>(Lifetime.Singleton);

            builder.Register<IEscapeWindowHandler, EscapeWindowHandler>(Lifetime.Singleton);

            builder.Register<GameplayTvHudInitializer>(Lifetime.Singleton);
        }
    }
}