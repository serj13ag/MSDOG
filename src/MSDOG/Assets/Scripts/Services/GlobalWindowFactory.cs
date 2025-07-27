using System;
using Constants;
using Data;
using UI.Windows;
using UnityEngine;
using VContainer;

namespace Services
{
    public class GlobalWindowFactory
    {
        private readonly IObjectResolver _container;
        private readonly AssetProviderService _assetProviderService;

        public GlobalWindowFactory(IObjectResolver container, AssetProviderService assetProviderService)
        {
            _container = container;
            _assetProviderService = assetProviderService;
        }

        public OptionsWindow CreateOptionsWindow(Transform canvasTransform)
        {
            return _assetProviderService.Instantiate<OptionsWindow>(AssetPaths.OptionsWindowPath, canvasTransform, _container);
        }
    }
}