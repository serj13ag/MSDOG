using UnityEngine;

namespace GameplayTvHud.DetailsZone
{
    public class FusionSlotHud : MonoBehaviour, IDetailsZone, IDetailDropTarget
    {
        [SerializeField] private FusionZoneHud _fusionZoneHud;

        private DetailView _detailView;

        public DetailView DetailView => _detailView;

        public void OnDetailDrop(DetailView detailView)
        {
            if (_detailView != null)
            {
                return;
            }

            detailView.SetCurrentZone(this);
        }

        public void Enter(DetailView detailView)
        {
            _detailView = detailView;
            detailView.transform.SetParent(transform);
            detailView.transform.localPosition = Vector3.zero;
            _fusionZoneHud.OnDetailEntersTheZone();
        }

        public void Exit(DetailView detailView)
        {
            _detailView = null;
            _fusionZoneHud.OnDetailExitsTheZone();
        }

        public void DestroyDetail()
        {
            var detailView = _detailView;
            Exit(detailView);
            Destroy(detailView.gameObject);
        }
    }
}