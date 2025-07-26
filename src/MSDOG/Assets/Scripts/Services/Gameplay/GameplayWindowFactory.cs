using Constants;
using UI.Windows;
using UnityEngine;
using VContainer;

namespace Services.Gameplay
{
    public class GameplayWindowFactory
    {
        private readonly IObjectResolver _container;
        private readonly AssetProviderService _assetProviderService;

        public GameplayWindowFactory(IObjectResolver container, AssetProviderService assetProviderService)
        {
            _container = container;
            _assetProviderService = assetProviderService;
        }

        public LoseWindow CreateLoseWindow(Transform canvasTransform)
        {
            return _assetProviderService.Instantiate<LoseWindow>(AssetPaths.LoseWindowPath, canvasTransform, _container);
        }

        public WinWindow CreateWinWindow(Transform canvasTransform)
        {
            return _assetProviderService.Instantiate<WinWindow>(AssetPaths.WinWindowPath, canvasTransform, _container);
        }

        public EscapeWindow CreateEscapeWindow(Transform canvasTransform)
        {
            return _assetProviderService.Instantiate<EscapeWindow>(AssetPaths.EscapeWindowPath, canvasTransform, _container);
        }
    }
}