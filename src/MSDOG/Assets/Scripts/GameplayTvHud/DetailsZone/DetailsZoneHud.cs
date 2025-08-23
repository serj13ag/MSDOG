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

            if (!_detailMediator.CanAddInactiveDetail())
            {
                return;
            }

            detailView.SetCurrentZone(this);
        }

        public void Enter(DetailView detailView)
        {
            detailView.transform.SetParent(_detailsGrid.transform);
            _detailViews.Add(detailView.Id, detailView);

            _detailMediator.AddInactiveDetail(detailView.Detail);
        }

        public void Exit(DetailView detailView)
        {
            _detailViews.Remove(detailView.Id);

            _detailMediator.RemoveInactiveDetail(detailView.Detail);
        }

        private void OnInactiveDetailCreated(object sender, DetailCreatedEventArgs e)
        {
            _detailViewFactory.CreateDetailView(e.Detail, this, _detailsGrid.transform, _parentCanvas);
        }

        private void OnDestroy()
        {
            _detailMediator.OnInactiveDetailCreated -= OnInactiveDetailCreated;
        }
    }
}