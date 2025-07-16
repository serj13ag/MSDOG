using Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.HUD.DetailsZone
{
    public class DetailPartHud : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _rectTransform;

        private Canvas _parentCanvas;

        private Transform _originalParent;
        private int _originalSiblingIndex;
        private AbilityData _abilityData;

        public AbilityData AbilityData => _abilityData;

        public void Init(AbilityData abilityData, Canvas parentCanvas)
        {
            _abilityData = abilityData;
            _parentCanvas = parentCanvas;
            _text.text = _abilityData.AbilityType.ToString();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _originalParent = transform.parent;
            _originalSiblingIndex = transform.GetSiblingIndex();

            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0.6f;

            transform.SetParent(_parentCanvas.transform);
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentCanvas.transform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out var localPoint);

            _rectTransform.localPosition = localPoint;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1f;

            if (transform.parent == _parentCanvas.transform)
            {
                transform.SetParent(_originalParent);
                transform.SetSiblingIndex(_originalSiblingIndex);
            }
        }
    }
}