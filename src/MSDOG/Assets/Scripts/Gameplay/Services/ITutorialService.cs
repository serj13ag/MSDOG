namespace Gameplay.Services
{
    public interface ITutorialService
    {
        void StartTrackPlayer(Player player);

        void OnCanCraft();
        void OnHasTwoSameDetails();
        void OnReloadNeeded();
        void OnFuseActionDisconnected();
        void OnLevelActivated();
    }
}