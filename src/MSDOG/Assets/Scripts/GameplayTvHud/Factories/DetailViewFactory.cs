using Core.Services;
using Gameplay;
using GameplayTvHud.DetailsZone;
using UnityEngine;

namespace GameplayTvHud.Factories
{
    public class DetailViewFactory : IDetailViewFactory
    {
        // TODO: add pool
        private readonly DetailPartHud _detailPartViewPrefab;

        public DetailViewFactory(IDataService dataService)
        {
            _detailPartViewPrefab = dataService.GetSettingsData().DetailPartViewPrefab;
        }

        public DetailPartHud CreateDetailPartView(Detail detail, Transform parentTransform, Canvas parentCanvas)
        {
            var detailPart = Object.Instantiate(_detailPartViewPrefab, parentTransform);
            detailPart.Init(detail, parentCanvas);
            return detailPart;
        }
    }
}