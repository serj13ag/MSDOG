using System;
using Gameplay;
using GameplayTvHud.Factories;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utility;
using Utility.Pools;
using VContainer;

namespace GameplayTvHud.DetailsZone
{
    public class DetailView : BasePooledObject, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _rectTransform;

        private IDetailViewFactory _detailViewFactory;

        private Canvas _parentCanvas;
        private Detail _detail;

        private IDetailsZone _currentDetailsZone;
        private DetailGhostView _dragGhost;

        public Guid Id => _detail.Id;
        public Detail Detail => _detail;

        [Inject]
        public void Construct(IDetailViewFactory detailViewFactory)
        {
            _detailViewFactory = detailViewFactory;
        }

        public void Init(Detail detail, IDetailsZone initialZone, Canvas parentCanvas)
        {
            _detail = detail;
            _parentCanvas = parentCanvas;
            _icon.sprite = detail.AbilityData.Icon;

            if (initialZone != null)
            {
                _currentDetailsZone = initialZone;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _canvasGroup.alpha = 0.4f;

            _dragGhost = _detailViewFactory.GetDetailGhost(_detail, _parentCanvas.transform);
            _dragGhost.transform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_dragGhost)
            {
                _dragGhost.FollowPointer(eventData, _parentCanvas.transform);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_dragGhost)
            {
                _dragGhost.Hide();
                _dragGhost = null;

                if (eventData.pointerCurrentRaycast.gameObject != null)
                {
                    if (eventData.pointerCurrentRaycast.gameObject
                        .TryGetComponentInHierarchy<IDetailDropTarget>(out var detailDropTarget))
                    {
                        detailDropTarget.OnDetailDrop(this);
                    }
                }
            }

            _canvasGroup.alpha = 1f;
        }

        public void SetCurrentZone(IDetailsZone newZone)
        {
            if (_currentDetailsZone != null && _currentDetailsZone != newZone)
            {
                _currentDetailsZone.Exit(this);
            }

            _currentDetailsZone = newZone;
            newZone.Enter(this);
        }

        public void Destruct()
        {
            Release();
        }

        protected override void Cleanup()
        {
            base.Cleanup();

            if (_dragGhost)
            {
                _dragGhost.Hide();
            }
        }
    }
}