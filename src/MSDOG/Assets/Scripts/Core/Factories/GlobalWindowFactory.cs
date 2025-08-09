using Constants;
using Core.Services;
using UI.Windows;
using UnityEngine;
using VContainer;

namespace Core.Factories
{
    public class GlobalWindowFactory : IGlobalWindowFactory
    {
        private readonly IObjectResolver _container;
        private readonly IAssetProviderService _assetProviderService;

        public GlobalWindowFactory(IObjectResolver container, IAssetProviderService assetProviderService)
        {
            _container = container;
            _assetProviderService = assetProviderService;
        }

        public OptionsWindow CreateOptionsWindow(Transform canvasTransform)
        {
            return _assetProviderService.Instantiate<OptionsWindow>(AssetPaths.OptionsWindowPath, canvasTransform, _container);
        }

        public CreditsWindow CreateCreditsWindow(Transform canvasTransform)
        {
            return _assetProviderService.Instantiate<CreditsWindow>(AssetPaths.CreditsWindowPath, canvasTransform, _container);
        }
    }
}