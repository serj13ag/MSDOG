using System;
using System.Collections.Generic;
using Constants;
using UI.Windows;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace Services.Gameplay
{
    public class GameplayWindowService
    {
        private readonly IObjectResolver _container;
        private readonly AssetProviderService _assetProviderService;

        private readonly Stack<IWindow> _activeWindows = new Stack<IWindow>();

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

        public void ShowEscape()
        {
            var escapeWindow =
                _assetProviderService.Instantiate<EscapeWindow>(AssetPaths.EscapeWindowPath, _rootCanvas.transform, _container);
            _activeWindows.Push(escapeWindow);
            escapeWindow.OnCloseRequested += CloseWindow;
        }

        public void CloseActiveWindow()
        {
            if (_activeWindows.Count == 0)
            {
                Debug.LogError("Has no active windows!");
                return;
            }

            var windowToClose = _activeWindows.Pop();
            Object.Destroy(windowToClose.GameObject);
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            CloseActiveWindow();
        }
    }
}