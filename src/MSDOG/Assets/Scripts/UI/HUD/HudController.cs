using Core;
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

        public void Init(Player player)
        {
            _healthBarHud.Init(player);
            _experienceBarHud.Init(player, _detailsZoneHud);
            _activeZoneHud.Init(player);
        }
    }
}