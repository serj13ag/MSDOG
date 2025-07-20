using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.HUD.DetailsZone
{
    public class FusionSlotHud : MonoBehaviour, IDetailsZone, IDropHandler
    {
        [SerializeField] private FusionZoneHud _fusionZoneHud;

        private DetailPartHud _detailPart;

        public DetailPartHud DetailPart => _detailPart;

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
            {
                return;
            }

            if (!eventData.pointerDrag.gameObject.TryGetComponent<DetailPartHud>(out var detailPart))
            {
                return;
            }

            if (_detailPart != null)
            {
                return;
            }

            detailPart.SetCurrentZone(this);
        }

        public void Enter(DetailPartHud detailPart)
        {
            _detailPart = detailPart;
            detailPart.transform.SetParent(transform);
            detailPart.transform.localPosition = Vector3.zero;
            _fusionZoneHud.OnDetailEntersTheZone();
        }

        public void Exit(DetailPartHud detailPart)
        {
            _detailPart = null;
            _fusionZoneHud.OnDetailExitsTheZone();
        }

        public void DestroyDetail()
        {
            var detailPartHud = _detailPart;
            Exit(detailPartHud);
            Destroy(detailPartHud.gameObject);
        }
    }
}