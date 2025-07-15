using Core;
using UnityEngine;

namespace UI.HUD
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private HealthBarHud _healthBarHud;
        [SerializeField] private ExperienceBarHud _experienceBarHud;

        public void Init(Player player)
        {
            _healthBarHud.Init(player);
            _experienceBarHud.Init(player);
        }
    }
}