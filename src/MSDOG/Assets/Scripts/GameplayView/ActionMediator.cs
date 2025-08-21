using Gameplay.Providers;
using Gameplay.Services;
using UnityEngine;

namespace GameplayView
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

    public class ActionMediator : IActionMediator
    {
        private readonly IPlayerProvider _playerProvider;
        private readonly ITutorialService _tutorialService;

        public bool PlayerHasNitro => _playerProvider.Player.HasNitro;

        public ActionMediator(IPlayerProvider playerProvider, ITutorialService tutorialService)
        {
            _playerProvider = playerProvider;
            _tutorialService = tutorialService;
        }

        public Vector3 GetPlayerPosition()
        {
            return _playerProvider.Player.transform.position;
        }

        public void ConnectFuse()
        {
            _playerProvider.Player.MovementSetActive(true);
        }

        public void DisconnectFuse()
        {
            _playerProvider.Player.MovementSetActive(false);
            _tutorialService.OnFuseActionDisconnected();
        }

        public void EnableNitro(float nitroMultiplier)
        {
            _playerProvider.Player.SetNitro(nitroMultiplier);
        }

        public void DisableNitro()
        {
            _playerProvider.Player.ResetNitro();
        }

        public void EnablePlayerAbilities()
        {
            _playerProvider.Player.AbilitiesSetActive(true);
        }

        public void DisablePlayerAbilities()
        {
            _playerProvider.Player.AbilitiesSetActive(false);
            _tutorialService.OnReloadNeeded();
        }
    }
}