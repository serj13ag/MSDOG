using Core.Models.Data;
using UI.HUD.DetailsZone;
using UnityEngine;
using VContainer;

namespace UI.HUD
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private Transform _canvasTransform;

        private DetailsZoneHud _detailsZoneHud;

        [Inject]
        public void Construct(DetailsZoneHud detailsZoneHud)
        {
            _detailsZoneHud = detailsZoneHud;
        }

        public void AddAbility(AbilityData abilityData)
        {
            _detailsZoneHud.CreateDetail(abilityData);
        }
    }
}