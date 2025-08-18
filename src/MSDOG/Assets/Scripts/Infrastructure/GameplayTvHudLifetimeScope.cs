using UI.HUD;
using UI.HUD.Actions;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure
{
    public class GameplayTvHudLifetimeScope : BaseLifetimeScope
    {
        [SerializeField] private HudController _hudController;
        [SerializeField] private HudActions _hudActions;

        protected override void ConfigureContainer(IContainerBuilder builder)
        {
            // TODO: register all child components and remove them from hud controller
            builder.RegisterComponent(_hudController).As<HudController>();
            builder.RegisterComponent(_hudActions).As<HudActions>();

            builder.Register<GameplayTvHudInitializer>(Lifetime.Singleton);
        }
    }
}