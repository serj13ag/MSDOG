using UnityEngine;

namespace GameplayView.Mediators
{
    public interface IActionMediator
    {
        bool PlayerHasNitro { get; }

        Vector3 GetPlayerPosition();

        void ConnectFuse();
        void DisconnectFuse();

        void EnableNitro(float nitroMultiplier);
        void DisableNitro();

        void EnablePlayerAbilities();
        void DisablePlayerAbilities();
    }
}