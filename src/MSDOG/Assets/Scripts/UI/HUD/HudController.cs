using Core;
using UnityEngine;

namespace UI.HUD
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private HealthBarHud _healthBarHud;

        public void Init(Player player)
        {
            _healthBarHud.Init(player);
        }
    }
}