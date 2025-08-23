using Core.Services;
using Gameplay;
using GameplayTvHud.DetailsZone;
using UnityEngine;

namespace GameplayTvHud.Factories
{
    public class DetailViewFactory : IDetailViewFactory
    {
        // TODO: add pool
        private readonly DetailView _detailViewPrefab;

        public DetailViewFactory(IDataService dataService)
        {
            _detailViewPrefab = dataService.GetSettingsData().DetailViewPrefab;
        }

        public DetailView CreateDetailPartView(Detail detail, Transform parentTransform, Canvas parentCanvas)
        {
            var detailPart = Object.Instantiate(_detailViewPrefab, parentTransform);
            detailPart.Init(detail, parentCanvas);
            return detailPart;
        }
    }
}