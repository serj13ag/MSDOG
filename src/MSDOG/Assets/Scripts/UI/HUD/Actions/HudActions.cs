using Core;
using Services;
using UnityEngine;
using VContainer;

namespace UI.HUD.Actions
{
    public class HudActions : MonoBehaviour
    {
        [SerializeField] private FuseAction _fuseAction;
        [SerializeField] private NitroAction _nitroAction;
        [SerializeField] private ReloadAction _reloadAction;

        private UpdateService _updateService;

        [Inject]
        public void Construct(UpdateService updateService)
        {
            _updateService = updateService;
        }

        public void Init(Player player)
        {
            _fuseAction.Init(player);
            _nitroAction.Init(player);
            _reloadAction.Init(player, _updateService);
        }
    }
}