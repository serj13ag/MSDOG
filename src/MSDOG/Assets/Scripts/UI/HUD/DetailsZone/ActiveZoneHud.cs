using System;
using System.Collections.Generic;
using Constants;
using Core.Services;
using Gameplay;
using Gameplay.Services;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace UI.HUD.DetailsZone
{
    public class ActiveZoneHud : MonoBehaviour, IDetailsZone, IDropHandler
    {
        [SerializeField] private Canvas _parentCanvas;
        [SerializeField] private Transform _grid;
        [SerializeField] private int _maxNumberOfActiveParts;

        private IAssetProviderService _assetProviderService;
        private IDetailService _detailService;

        private readonly Dictionary<Guid, DetailPartHud> _detailParts = new Dictionary<Guid, DetailPartHud>();

        [Inject]
        public void Construct(IAssetProviderService assetProviderService, IDetailService detailService)
        {
            _detailService = detailService;
            _assetProviderService = assetProviderService;
        }

        public void Init()
        {
            foreach (var activeDetail in _detailService.ActiveDetails)
            {
                var detailPart = _assetProviderService.Instantiate<DetailPartHud>(AssetPaths.DetailPartPrefabPath, _grid.transform);
                detailPart.Init(activeDetail.Value, _parentCanvas);
                detailPart.SetCurrentZone(this);
            }

            _detailService.OnActiveDetailCreated += OnActiveDetailCreated;
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
            _detailService.ActivateDetail(detailPart.Id);
        }

        public void Exit(DetailPartHud detailPart)
        {
            _detailParts.Remove(detailPart.Id);
            _detailService.DeactivateDetail(detailPart.Id);
        }

        public IEnumerable<DetailPartHud> GetDetailParts()
        {
            foreach (var detailPart in _detailParts)
            {
                yield return detailPart.Value;
            }
        }
        
        private void OnActiveDetailCreated(object sender, DetailCreatedEventArgs e)
        {
            var detailPart = _assetProviderService.Instantiate<DetailPartHud>(AssetPaths.DetailPartPrefabPath, _grid.transform);
            detailPart.Init(e.Detail, _parentCanvas);
            detailPart.SetCurrentZone(this);
        }

        private void OnDestroy()
        {
            _detailService.OnActiveDetailCreated -= OnActiveDetailCreated;
        }
    }
}