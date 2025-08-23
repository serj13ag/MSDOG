using System;
using System.Collections.Generic;
using Gameplay;
using GameplayTvHud.Factories;
using GameplayTvHud.Mediators;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace GameplayTvHud.DetailsZone
{
    public class ActiveZoneHud : MonoBehaviour, IDetailsZone, IDropHandler
    {
        [SerializeField] private Canvas _parentCanvas;
        [SerializeField] private Transform _grid;
        [SerializeField] private int _maxNumberOfActiveParts;

        private IDetailMediator _detailMediator;
        private IDetailViewFactory _detailViewFactory;

        private readonly Dictionary<Guid, DetailView> _detailParts = new Dictionary<Guid, DetailView>();

        [Inject]
        public void Construct(IDetailMediator detailMediator, IDetailViewFactory detailViewFactory)
        {
            _detailViewFactory = detailViewFactory;
            _detailMediator = detailMediator;

            detailMediator.OnActiveDetailCreated += OnActiveDetailCreated;
        }

        public void CreateStarActiveDetails()
        {
            foreach (var activeDetail in _detailMediator.GetActiveDetails())
            {
                CreateDetailPart(activeDetail);
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
            {
                return;
            }

            if (!eventData.pointerDrag.gameObject.TryGetComponent<DetailView>(out var detailPart))
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

        public void Enter(DetailView detailPart)
        {
            _detailParts.Add(detailPart.Id, detailPart);
            detailPart.transform.SetParent(_grid.transform);
            _detailMediator.ActivateDetail(detailPart.Detail);
        }

        public void Exit(DetailView detailPart)
        {
            _detailParts.Remove(detailPart.Id);
            _detailMediator.DeactivateDetail(detailPart.Detail);
        }

        private void OnActiveDetailCreated(object sender, DetailCreatedEventArgs e)
        {
            CreateDetailPart(e.Detail);
        }

        private void CreateDetailPart(Detail detail)
        {
            var detailPart = _detailViewFactory.CreateDetailPartView(detail, _grid.transform, _parentCanvas);
            detailPart.SetCurrentZone(this);
        }

        private void OnDestroy()
        {
            _detailMediator.OnActiveDetailCreated -= OnActiveDetailCreated;
        }
    }
}