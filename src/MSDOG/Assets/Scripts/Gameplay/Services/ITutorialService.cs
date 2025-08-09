namespace Gameplay.Services
{
    public interface ITutorialService
    {
        void SetPlayer(Player player);

        void OnCanCraft();
        void OnHasTwoSameDetails();
        void OnReloadNeeded();
        void OnFuseActionDisconnected();
        void OnLevelActivated();
    }
}