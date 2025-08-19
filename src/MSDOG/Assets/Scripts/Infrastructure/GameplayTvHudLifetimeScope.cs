using UI.HUD;
using UI.HUD.Actions;
using UI.HUD.DetailsZone;
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

        [SerializeField] private HudController _hudController;
        [SerializeField] private HudActions _hudActions;

        protected override void ConfigureContainer(IContainerBuilder builder)
        {
            // TODO: register all child components and remove them from hud controller
            builder.RegisterComponent(_healthBarHud).As<HealthBarHud>();
            builder.RegisterComponent(_experienceBarHud).As<ExperienceBarHud>();

            builder.RegisterComponent(_detailsZoneHud).As<DetailsZoneHud>();
            builder.RegisterComponent(_activeZoneHud).As<ActiveZoneHud>();
            builder.RegisterComponent(_destructZoneHud).As<DestructZoneHud>();
            builder.RegisterComponent(_fusionZoneHud).As<FusionZoneHud>();

            builder.RegisterComponent(_hudController).As<HudController>();
            builder.RegisterComponent(_hudActions).As<HudActions>();

            builder.Register<IEscapeWindowHandler, EscapeWindowHandler>(Lifetime.Singleton);

            builder.Register<GameplayTvHudInitializer>(Lifetime.Singleton);
        }
    }
}