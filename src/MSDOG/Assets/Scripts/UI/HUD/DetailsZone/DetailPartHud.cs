using Core.Details;
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
        private Detail _detail;

        public Detail Detail => _detail;

        public void Init(Detail detail, Canvas parentCanvas)
        {
            _detail = detail;
            _parentCanvas = parentCanvas;
            _text.text = detail.ToString();
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