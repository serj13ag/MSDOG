using Core.Services;
using Gameplay;
using GameplayTvHud.DetailsZone;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GameplayTvHud.Factories
{
    public class DetailViewFactory : IDetailViewFactory
    {
        // TODO: add pool
        private readonly IObjectResolver _container;

        private readonly DetailView _detailViewPrefab;
        private readonly DetailGhostView _detailGhostViewPrefab;

        public DetailViewFactory(IObjectResolver container, IDataService dataService)
        {
            _container = container;
            _detailViewPrefab = dataService.GetSettingsData().DetailViewPrefab;
            _detailGhostViewPrefab = dataService.GetSettingsData().DetailGhostViewPrefab;
        }

        public DetailView CreateDetailView(Detail detail, Transform parentTransform, Canvas parentCanvas)
        {
            var detailView = _container.Instantiate(_detailViewPrefab, parentTransform);
            detailView.Init(detail, parentCanvas);
            return detailView;
        }

        public DetailGhostView GetDetailGhost(Detail detail, Transform parentTransform)
        {
            var detailGhostView = _container.Instantiate(_detailGhostViewPrefab, parentTransform);
            detailGhostView.Init(detail);
            return detailGhostView;
        }
    }
}