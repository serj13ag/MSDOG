using System;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.HUD.DetailsZone
{
    public class DetailPartHud : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private DetailPartGhostHud _ghostDetailPrefab;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _icon;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _rectTransform;

        private Canvas _parentCanvas;

        private AbilityData _abilityData;
        private IDetailsZone _currentDetailsZone;
        private DetailPartGhostHud _dragGhost;

        public Guid Id { get; private set; }
        public AbilityData AbilityData => _abilityData;

        public void Init(AbilityData abilityData, Canvas parentCanvas)
        {
            Id = Guid.NewGuid();
            _abilityData = abilityData;
            _parentCanvas = parentCanvas;
            _icon.sprite = _abilityData.Icon;
            //_text.text = $"{_abilityData.AbilityType}_{_abilityData.Level}";
            _text.text = string.Empty;
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

            _dragGhost = Instantiate(_ghostDetailPrefab, _parentCanvas.transform);
            _dragGhost.Init(_abilityData);
            _dragGhost.transform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_dragGhost)
            {
                return;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentCanvas.transform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out var localPoint);

            ((RectTransform)_dragGhost.transform).localPosition = localPoint;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Destroy(_dragGhost.gameObject);
            _dragGhost = null;

            _canvasGroup.alpha = 1f;
        }

        private void OnDestroy()
        {
            if (_dragGhost)
            {
                Destroy(_dragGhost.gameObject);
            }
        }
    }
}