using Core.Controllers;
using Core.Sounds;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace UI.HUD.DetailsZone
{
    public class DestructZoneHud : MonoBehaviour, IDetailsZone, IDropHandler
    {
        private ISoundController _soundController;
        private IDetailMediator _detailMediator;

        [Inject]
        public void Construct(ISoundController soundController, IDetailMediator detailMediator)
        {
            _detailMediator = detailMediator;
            _soundController = soundController;
        }

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
            detailPart.Destruct();

            _detailMediator.DetailDestructed();

            _soundController.PlaySfx(SfxType.Destructor);
        }

        public void Exit(DetailPartHud detailPart)
        {
            Debug.LogError("Can't exit from that zone!");
        }
    }
}