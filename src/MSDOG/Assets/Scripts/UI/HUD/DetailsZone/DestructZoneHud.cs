using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.HUD.DetailsZone
{
    public class DestructZoneHud : MonoBehaviour, IDetailsZone, IDropHandler
    {
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

            detailPart.SetCurrentZone(this);
        }

        public void Enter(DetailPartHud detailPart)
        {
            Destroy(detailPart.gameObject);
        }

        public void Exit(DetailPartHud detailPart)
        {
            throw new System.NotImplementedException();
        }
    }
}