using System;
using System.Collections.Generic;
using Gameplay;
using GameplayTvHud.Factories;
using GameplayTvHud.Mediators;
using UnityEngine;
using VContainer;

namespace GameplayTvHud.DetailsZone
{
    public class ActiveZoneHud : MonoBehaviour, IDetailsZone, IDetailDropTarget
    {
        [SerializeField] private Canvas _parentCanvas;
        [SerializeField] private Transform _grid;

        private IDetailMediator _detailMediator;
        private IDetailViewFactory _detailViewFactory;

        private readonly Dictionary<Guid, DetailView> _detailViews = new Dictionary<Guid, DetailView>();

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
                CreateDetailView(activeDetail);
            }
        }

        public void OnDetailDrop(DetailView detailView)
        {
            if (_detailViews.ContainsKey(detailView.Id))
            {
                return;
            }

            if (!_detailMediator.CanAddActiveDetail())
            {
                return;
            }

            detailView.SetCurrentZone(this);
        }

        public void Enter(DetailView detailView)
        {
            _detailViews.Add(detailView.Id, detailView);
            detailView.transform.SetParent(_grid.transform);

            _detailMediator.AddActiveDetail(detailView.Detail);
        }

        public void Exit(DetailView detailView)
        {
            _detailViews.Remove(detailView.Id);

            _detailMediator.RemoveActiveDetail(detailView.Detail);
        }

        private void OnActiveDetailCreated(object sender, DetailCreatedEventArgs e)
        {
            CreateDetailView(e.Detail);
        }

        private void CreateDetailView(Detail detail)
        {
            _detailViewFactory.CreateDetailView(detail, this, _grid.transform, _parentCanvas);
        }

        private void OnDestroy()
        {
            _detailMediator.OnActiveDetailCreated -= OnActiveDetailCreated;
        }
    }
}