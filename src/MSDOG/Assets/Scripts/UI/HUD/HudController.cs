using Core;
using Services;
using UI.HUD.DetailsZone;
using UnityEngine;

namespace UI.HUD
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private HealthBarHud _healthBarHud;
        [SerializeField] private ExperienceBarHud _experienceBarHud;
        [SerializeField] private DetailsZoneHud _detailsZoneHud;
        [SerializeField] private ActiveZoneHud _activeZoneHud;

        public void Init(Player player, DataService dataService)
        {
            _healthBarHud.Init(player);
            _experienceBarHud.Init(player, dataService, _detailsZoneHud);
            _activeZoneHud.Init(player);
        }
    }
}