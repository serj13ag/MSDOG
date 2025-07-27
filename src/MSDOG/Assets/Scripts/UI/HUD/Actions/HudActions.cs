using Core;
using UnityEngine;

namespace UI.HUD.Actions
{
    public class HudActions : MonoBehaviour
    {
        [SerializeField] private FuseAction _fuseAction;
        [SerializeField] private NitroAction _nitroAction;

        public void Init(Player player)
        {
            _fuseAction.Init(player);
            _nitroAction.Init(player);
        }
    }
}