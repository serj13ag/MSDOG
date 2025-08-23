using Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameplayTvHud.DetailsZone
{
    public class DetailGhostView : MonoBehaviour
    {
        [SerializeField] private Image _icon;

        public void Init(Detail detail)
        {
            _icon.sprite = detail.AbilityData.Icon;
        }

        public void FollowPointer(PointerEventData eventData, Transform parentCanvasTransform)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvasTransform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out var localPoint);

            ((RectTransform)transform).localPosition = localPoint;
        }

        public void Hide()
        {
            Destroy(gameObject);
        }
    }
}