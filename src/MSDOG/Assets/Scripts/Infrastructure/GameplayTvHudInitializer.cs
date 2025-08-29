using GameplayTvHud.Actions.Fuse;
using GameplayTvHud.DetailsZone;

namespace Infrastructure
{
    public class GameplayTvHudInitializer
    {
        private readonly ActiveZoneHud _activeZoneHud;
        private readonly FuseAction _fuseAction;

        public GameplayTvHudInitializer(ActiveZoneHud activeZoneHud, FuseAction fuseAction)
        {
            _activeZoneHud = activeZoneHud;
            _fuseAction = fuseAction;
        }

        public void Start()
        {
            _activeZoneHud.CreateStarActiveDetails();
            _fuseAction.StartConnected();
        }
    }
}