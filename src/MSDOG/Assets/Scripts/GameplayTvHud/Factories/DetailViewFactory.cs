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

        private DetailGhostView _detailGhostView;

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
            if (!_detailGhostView)
            {
                _detailGhostView = _container.Instantiate(_detailGhostViewPrefab, parentTransform);
                _detailGhostView.SetReleaseAction(ReleaseDetailGhostView);
            }

            _detailGhostView.Init(detail);
            _detailGhostView.gameObject.SetActive(true);
            return _detailGhostView;
        }

        private void ReleaseDetailGhostView()
        {
            _detailGhostView.gameObject.SetActive(false);
        }
    }
}