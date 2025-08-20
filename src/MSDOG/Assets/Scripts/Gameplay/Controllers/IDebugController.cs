using System;
using UI.HUD.DetailsZone;

namespace Gameplay.Controllers
{
    public interface IDebugController
    {
        bool DebugHpIsVisible { get; }

        event EventHandler<EventArgs> OnShowDebugHealthBar;
        event EventHandler<EventArgs> OnHideDebugHealthBar;
        event EventHandler<EventArgs> OnForceSpawnEnemiesRequested;
        event EventHandler<EventArgs> OnKillAllEnemiesRequested;
    }
}