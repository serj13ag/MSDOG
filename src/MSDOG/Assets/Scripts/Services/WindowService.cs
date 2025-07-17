using Constants;
using UI.Windows;
using UnityEngine;

namespace Services
{
    public class WindowService
    {
        private readonly AssetProviderService _assetProviderService;

        private Canvas _rootCanvas;

        public WindowService(AssetProviderService assetProviderService)
        {
            _assetProviderService = assetProviderService;
        }

        public void CreateRootCanvas()
        {
            _rootCanvas = _assetProviderService.Instantiate<Canvas>(AssetPaths.UiRootCanvasPath);
        }

        public void CreateLoseWindow()
        {
            _assetProviderService.Instantiate<LoseWindow>(AssetPaths.LoseWindowPath, _rootCanvas.transform);
        }

        public void CreateWinWindow()
        {
            _assetProviderService.Instantiate<WinWindow>(AssetPaths.WinWindowPath, _rootCanvas.transform);
        }
    }
}