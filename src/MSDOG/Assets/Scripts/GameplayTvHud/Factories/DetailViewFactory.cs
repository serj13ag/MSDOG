using Core.Services;
using Gameplay;
using GameplayTvHud.DetailsZone;
using UnityEngine;
using Utility.Pools;
using VContainer;
using VContainer.Unity;

namespace GameplayTvHud.Factories
{
    public class DetailViewFactory : IDetailViewFactory
    {
        private readonly IObjectResolver _container;

        private readonly DetailGhostView _detailGhostViewPrefab;
        private readonly GameObjectPool<DetailView> _detailViewPool;

        private DetailGhostView _detailGhostView;

        public DetailViewFactory(IObjectResolver container, IDataService dataService)
        {
            _container = container;

            var settingsData = dataService.GetSettingsData();
            _detailGhostViewPrefab = settingsData.DetailGhostViewPrefab;

            _detailViewPool = new GameObjectPool<DetailView>(() => container.Instantiate(settingsData.DetailViewPrefab));
        }

        public DetailView CreateDetailView(Detail detail, IDetailsZone initialZone, Transform parentTransform,
            Canvas parentCanvas)
        {
            var detailView = _detailViewPool.Get();
            detailView.transform.SetParent(parentTransform, false);
            detailView.Init(detail, initialZone, parentCanvas);
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