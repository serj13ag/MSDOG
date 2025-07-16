using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.HUD.DetailsZone
{
    public class DetailsZoneHud : MonoBehaviour, IDetailsZone, IDropHandler
    {
        [SerializeField] private Canvas _parentCanvas;
        [SerializeField] private DetailPartHud _detailPartPrefab;
        [SerializeField] private GridLayoutGroup _detailsGrid;

        private readonly List<DetailPartHud> _detailParts = new List<DetailPartHud>();

        public void CreateDetail(AbilityData abilityData)
        {
            var detailPart = Instantiate(_detailPartPrefab, _detailsGrid.transform);
            detailPart.Init(abilityData, _parentCanvas);
            detailPart.SetCurrentZone(this);
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

            if (_detailParts.Contains(detailPart))
            {
                return;
            }

            detailPart.SetCurrentZone(this);
        }

        public void Enter(DetailPartHud detailPart)
        {
            detailPart.transform.SetParent(_detailsGrid.transform);
            _detailParts.Add(detailPart);
        }

        public void Exit(DetailPartHud detailPart)
        {
            _detailParts.Remove(detailPart);
        }
    }
}