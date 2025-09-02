using Gameplay.Interfaces;

namespace Gameplay.Services
{
    public interface ITutorialService
    {
        void StartTrackTarget(IEntityWithHealth entityWithHealth);

        void OnCanCraft();
        void OnHasDetailsWithSimilarAbilities();
        void OnReloadNeeded();
        void OnFuseActionDisconnected();
        void OnLevelActivated();
    }
}