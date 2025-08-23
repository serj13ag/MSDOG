using System;
using System.Collections.Generic;
using Gameplay;
using GameplayTvHud.Factories;
using GameplayTvHud.Mediators;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace GameplayTvHud.DetailsZone
{
    public class DetailsZoneHud : MonoBehaviour, IDetailsZone, IDetailDropTarget
    {
        [SerializeField] private Canvas _parentCanvas;
        [SerializeField] private GridLayoutGroup _detailsGrid;
        [SerializeField] private int _maxNumberOfParts; // TODO: settings?

        private IDetailMediator _detailMediator;
        private IDetailViewFactory _detailViewFactory;

        private readonly Dictionary<Guid, DetailView> _detailViews = new Dictionary<Guid, DetailView>();

        [Inject]
        public void Construct(IDetailMediator detailMediator, IDetailViewFactory detailViewFactory)
        {
            _detailViewFactory = detailViewFactory;
            _detailMediator = detailMediator;

            detailMediator.OnInactiveDetailCreated += OnInactiveDetailCreated;
        }

        public void OnDetailDrop(DetailView detailView)
        {
            if (_detailViews.ContainsKey(detailView.Id))
            {
                return;
            }

            if (_detailViews.Count >= _maxNumberOfParts)
            {
                return;
            }

            detailView.SetCurrentZone(this);
        }

        public void Enter(DetailView detailView)
        {
            detailView.transform.SetParent(_detailsGrid.transform);
            _detailViews.Add(detailView.Id, detailView);
        }

        public void Exit(DetailView detailView)
        {
            _detailViews.Remove(detailView.Id);
        }

        private void OnInactiveDetailCreated(object sender, DetailCreatedEventArgs e)
        {
            var detailView = _detailViewFactory.CreateDetailView(e.Detail, _detailsGrid.transform, _parentCanvas);
            detailView.SetCurrentZone(this);
        }

        private void OnDestroy()
        {
            _detailMediator.OnInactiveDetailCreated -= OnInactiveDetailCreated;
        }
    }
}