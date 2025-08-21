using GameplayTvHud.Actions;
using GameplayTvHud.DetailsZone;
using GameplayTvHud.UI;

namespace Infrastructure
{
    public class GameplayTvHudInitializer
    {
        private readonly HealthBarHud _healthBarHud;
        private readonly ExperienceBarHud _experienceBarHud;
        private readonly ActiveZoneHud _activeZoneHud;
        private readonly DetailsZoneHud _detailsZoneHud;
        private readonly FuseAction _fuseAction;
        private readonly NitroAction _nitroAction;
        private readonly ReloadAction _reloadAction;

        public GameplayTvHudInitializer(HealthBarHud healthBarHud,
            ExperienceBarHud experienceBarHud,
            ActiveZoneHud activeZoneHud,
            DetailsZoneHud detailsZoneHud,
            FuseAction fuseAction,
            NitroAction nitroAction,
            ReloadAction reloadAction)
        {
            _healthBarHud = healthBarHud;
            _experienceBarHud = experienceBarHud;
            _activeZoneHud = activeZoneHud;
            _detailsZoneHud = detailsZoneHud;
            _fuseAction = fuseAction;
            _nitroAction = nitroAction;
            _reloadAction = reloadAction;
        }

        public void Start()
        {
            _healthBarHud.Init();
            _experienceBarHud.Init();

            _activeZoneHud.Init();
            _detailsZoneHud.Init();

            _fuseAction.Init();
            _nitroAction.Init();
            _reloadAction.Init();
        }
    }
}