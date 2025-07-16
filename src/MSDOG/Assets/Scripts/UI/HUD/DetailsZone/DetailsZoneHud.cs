using Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD.DetailsZone
{
    public class DetailsZoneHud : MonoBehaviour
    {
        [SerializeField] private Canvas _parentCanvas;
        [SerializeField] private DetailPartHud _detailPartPrefab;
        [SerializeField] private GridLayoutGroup _detailsGrid;

        public void CreateDetail(AbilityData abilityData)
        {
            var detailPart = Instantiate(_detailPartPrefab,  _detailsGrid.transform);
            detailPart.Init(abilityData, _parentCanvas);
        }
    }
}