using System;
using System.Collections.Generic;
using System.Linq;
using Constants;
using Core.Services;
using Gameplay;
using Gameplay.Services;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace UI.HUD.DetailsZone
{
    public class DetailsZoneHud : MonoBehaviour, IDetailsZone, IDropHandler
    {
        [SerializeField] private Canvas _parentCanvas;
        [SerializeField] private GridLayoutGroup _detailsGrid;
        [SerializeField] private int _maxNumberOfParts;

        private IAssetProviderService _assetProviderService;
        private ITutorialService _tutorialService;
        private ActiveZoneHud _activeZoneHud;
        private IDetailService _detailService;

        private readonly Dictionary<Guid, DetailPartHud> _detailParts = new Dictionary<Guid, DetailPartHud>();

        [Inject]
        public void Construct(IAssetProviderService assetProviderService, ITutorialService tutorialService,
            ActiveZoneHud activeZoneHud, IDetailService detailService)
        {
            _detailService = detailService;
            _tutorialService = tutorialService;
            _assetProviderService = assetProviderService;
            _activeZoneHud = activeZoneHud;
        }

        public void Init()
        {
            _detailService.OnInactiveDetailCreated += OnInactiveDetailCreated;
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

            if (HasSameDetails())
            {
                _tutorialService.OnHasTwoSameDetails();
            }
        }

        public void Exit(DetailPartHud detailPart)
        {
            _detailParts.Remove(detailPart.Id);
        }

        private void OnInactiveDetailCreated(object sender, DetailCreatedEventArgs e)
        {
            var detailPart = _assetProviderService.Instantiate<DetailPartHud>(AssetPaths.DetailPartPrefabPath, _detailsGrid.transform);
            detailPart.Init(e.Detail, _parentCanvas);
            detailPart.SetCurrentZone(this);
        }

        private bool HasSameDetails()
        {
            // TODO: refactor
            var allDetails = new List<DetailPartHud>(_detailParts.Values);
            allDetails.AddRange(_activeZoneHud.GetDetailParts());
            return allDetails
                .GroupBy(a => new { a.AbilityData.AbilityType, a.AbilityData.Level })
                .Any(g => g.Count() > 1);
        }

        private void OnDestroy()
        {
            _detailService.OnInactiveDetailCreated -= OnInactiveDetailCreated;
        }
    }
}