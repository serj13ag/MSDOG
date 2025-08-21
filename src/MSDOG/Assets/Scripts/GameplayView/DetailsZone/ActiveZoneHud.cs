using System;
using System.Collections.Generic;
using Constants;
using Core.Services;
using Gameplay;
using GameplayView.Mediators;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace GameplayView.DetailsZone
{
    public class ActiveZoneHud : MonoBehaviour, IDetailsZone, IDropHandler
    {
        [SerializeField] private Canvas _parentCanvas;
        [SerializeField] private Transform _grid;
        [SerializeField] private int _maxNumberOfActiveParts;

        private IAssetProviderService _assetProviderService;
        private IDetailMediator _detailMediator;

        private readonly Dictionary<Guid, DetailPartHud> _detailParts = new Dictionary<Guid, DetailPartHud>();

        [Inject]
        public void Construct(IAssetProviderService assetProviderService, IDetailMediator detailMediator)
        {
            _detailMediator = detailMediator;
            _assetProviderService = assetProviderService;
        }

        public void Init()
        {
            foreach (var activeDetail in _detailMediator.GetActiveDetails())
            {
                var detailPart = _assetProviderService.Instantiate<DetailPartHud>(AssetPaths.DetailPartPrefabPath, _grid.transform);
                detailPart.Init(activeDetail, _parentCanvas);
                detailPart.SetCurrentZone(this);
            }

            _detailMediator.OnActiveDetailCreated += OnActiveDetailCreated;
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

            if (_detailParts.Count >= _maxNumberOfActiveParts)
            {
                return;
            }

            detailPart.SetCurrentZone(this);
        }

        public void Enter(DetailPartHud detailPart)
        {
            _detailParts.Add(detailPart.Id, detailPart);
            detailPart.transform.SetParent(_grid.transform);
            _detailMediator.ActivateDetail(detailPart.Detail);
        }

        public void Exit(DetailPartHud detailPart)
        {
            _detailParts.Remove(detailPart.Id);
            _detailMediator.DeactivateDetail(detailPart.Detail);
        }
        
        private void OnActiveDetailCreated(object sender, DetailCreatedEventArgs e)
        {
            var detailPart = _assetProviderService.Instantiate<DetailPartHud>(AssetPaths.DetailPartPrefabPath, _grid.transform);
            detailPart.Init(e.Detail, _parentCanvas);
            detailPart.SetCurrentZone(this);
        }

        private void OnDestroy()
        {
            _detailMediator.OnActiveDetailCreated -= OnActiveDetailCreated;
        }
    }
}