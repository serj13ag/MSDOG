using System;
using System.Collections.Generic;
using Constants;
using UI.Windows;
using UnityEngine;
using VContainer;

namespace Services
{
    public class WindowService : PersistentMonoService
    {
        [SerializeField] private Canvas _canvas;

        private IObjectResolver _container;
        private AssetProviderService _assetProviderService;

        private readonly Stack<IWindow> _activeWindows = new Stack<IWindow>();

        [Inject]
        public void Construct(IObjectResolver container, AssetProviderService assetProviderService)
        {
            _container = container;
            _assetProviderService = assetProviderService;
        }

        public void ShowOptions()
        {
            var optionsWindow =
                _assetProviderService.Instantiate<OptionsWindow>(AssetPaths.OptionsWindowPath, _canvas.transform, _container);
            _activeWindows.Push(optionsWindow);
            optionsWindow.OnCloseRequested += CloseWindow;
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            CloseActiveWindow();
        }

        private void CloseActiveWindow()
        {
            if (_activeWindows.Count == 0)
            {
                Debug.LogError("Has no active windows!");
                return;
            }

            var windowToClose = _activeWindows.Pop();
            Destroy(windowToClose.GameObject);
        }
    }
}