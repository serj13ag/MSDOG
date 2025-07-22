using Core;
using UnityEngine;

namespace UI.HUD.Actions
{
    public class HudActions : MonoBehaviour
    {
        [SerializeField] private FuseAction _fuseAction;

        public void Init(Player player)
        {
            _fuseAction.Init(player);
        }
    }
}