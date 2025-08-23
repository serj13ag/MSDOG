using System;
using Core.Models.Data;
using Gameplay;
using GameplayTvHud.Factories;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace GameplayTvHud.DetailsZone
{
    public class DetailView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
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
        public AbilityData AbilityData => _detail.AbilityData;
        public Detail Detail => _detail;

        [Inject]
        public void Construct(IDetailViewFactory detailViewFactory)
        {
            _detailViewFactory = detailViewFactory;
        }

        public void Init(Detail detail, Canvas parentCanvas)
        {
            _detail = detail;
            _parentCanvas = parentCanvas;
            _icon.sprite = detail.AbilityData.Icon;
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
            Destroy(gameObject);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _canvasGroup.alpha = 0.4f;

            // TODO: refactor
            _dragGhost = _detailViewFactory.GetDetailGhost(_detail, _parentCanvas.transform);
            _dragGhost.transform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_dragGhost)
            {
                return;
            }

            _dragGhost.FollowPointer(eventData, _parentCanvas.transform);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _dragGhost.Hide();
            _dragGhost = null;

            _canvasGroup.alpha = 1f;
        }

        private void OnDestroy()
        {
            if (_dragGhost)
            {
                _dragGhost.Hide();
            }
        }
    }
}