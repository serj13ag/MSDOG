using Constants;
using UI.Windows;
using UnityEngine;
using VContainer;

namespace Services.Gameplay
{
    public class GameplayWindowService
    {
        private readonly IObjectResolver _container;
        private readonly AssetProviderService _assetProviderService;

        private Canvas _rootCanvas;

        public GameplayWindowService(IObjectResolver container, AssetProviderService assetProviderService)
        {
            _container = container;
            _assetProviderService = assetProviderService;
        }

        public void CreateRootCanvas()
        {
            _rootCanvas = _assetProviderService.Instantiate<Canvas>(AssetPaths.UiRootCanvasPath);
        }

        public void CreateLoseWindow()
        {
            _assetProviderService.Instantiate<LoseWindow>(AssetPaths.LoseWindowPath, _rootCanvas.transform, _container);
        }

        public void CreateWinWindow()
        {
            _assetProviderService.Instantiate<WinWindow>(AssetPaths.WinWindowPath, _rootCanvas.transform, _container);
        }
    }
}