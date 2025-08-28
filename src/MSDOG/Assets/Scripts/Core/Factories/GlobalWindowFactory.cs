using Windows;
using Core.Models.Data;
using Core.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core.Factories
{
    public class GlobalWindowFactory : IGlobalWindowFactory
    {
        private readonly IObjectResolver _container;
        private readonly WindowsData _windowsData;

        public GlobalWindowFactory(IObjectResolver container, IDataService dataService)
        {
            _container = container;
            _windowsData = dataService.GetWindowsData();
        }

        public OptionsWindow CreateOptionsWindow(Transform canvasTransform)
        {
            return _container.Instantiate(_windowsData.OptionsWindowPrefab, canvasTransform);
        }

        public CreditsWindow CreateCreditsWindow(Transform canvasTransform)
        {
            return _container.Instantiate(_windowsData.CreditsWindowPrefab, canvasTransform);
        }
    }
}