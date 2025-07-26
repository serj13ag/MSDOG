using System;
using Constants;
using UI.Windows;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace Services
{
    public class WindowService
    {
        private readonly IObjectResolver _container;
        private readonly AssetProviderService _assetProviderService;

        public WindowService(IObjectResolver container, AssetProviderService assetProviderService)
        {
            _container = container;
            _assetProviderService = assetProviderService;
        }

        public void ShowOptions(Transform parent)
        {
            var optionsWindow =
                _assetProviderService.Instantiate<OptionsWindow>(AssetPaths.OptionsWindowPath, parent, _container);
            optionsWindow.OnCloseRequested += CloseWindow;
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            var window = (OptionsWindow)sender;
            Object.Destroy(window.gameObject);
        }
    }
}