using System;
using System.Collections.Generic;
using Constants;
using Core.Services;
using Gameplay;
using GameplayTvHud.Mediators;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace GameplayTvHud.DetailsZone
{
    public class DetailsZoneHud : MonoBehaviour, IDetailsZone, IDropHandler
    {
        [SerializeField] private Canvas _parentCanvas;
        [SerializeField] private GridLayoutGroup _detailsGrid;
        [SerializeField] private int _maxNumberOfParts;

        private IAssetProviderService _assetProviderService;
        private IDetailMediator _detailMediator;

        private readonly Dictionary<Guid, DetailPartHud> _detailParts = new Dictionary<Guid, DetailPartHud>();

        [Inject]
        public void Construct(IAssetProviderService assetProviderService, IDetailMediator detailMediator)
        {
            _detailMediator = detailMediator;
            _assetProviderService = assetProviderService;

            detailMediator.OnInactiveDetailCreated += OnInactiveDetailCreated;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
            {
                return;
            }

            if (!eventData.pointerDrag.gameObject.TryGetComponent<DetailPartHud>(out var detailPart))
            {
                return;
            }

            if (_detailParts.ContainsKey(detailPart.Id))
            {
                return;
            }

            if (_detailParts.Count >= _maxNumberOfParts)
            {
                return;
            }

            detailPart.SetCurrentZone(this);
        }

        public void Enter(DetailPartHud detailPart)
        {
            detailPart.transform.SetParent(_detailsGrid.transform);
            _detailParts.Add(detailPart.Id, detailPart);
        }

        public void Exit(DetailPartHud detailPart)
        {
            _detailParts.Remove(detailPart.Id);
        }

        private void OnInactiveDetailCreated(object sender, DetailCreatedEventArgs e)
        {
            // TODO: to factory
            var detailPart = _assetProviderService.Instantiate<DetailPartHud>(AssetPaths.DetailPartPrefabPath, _detailsGrid.transform);
            detailPart.Init(e.Detail, _parentCanvas);
            detailPart.SetCurrentZone(this);
        }

        private void OnDestroy()
        {
            _detailMediator.OnInactiveDetailCreated -= OnInactiveDetailCreated;
        }
    }
}