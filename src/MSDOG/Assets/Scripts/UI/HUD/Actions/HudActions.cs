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
        private SoundService _soundService;

        [Inject]
        public void Construct(UpdateService updateService, SoundService soundService)
        {
            _soundService = soundService;
            _updateService = updateService;
        }

        public void Init(Player player)
        {
            _fuseAction.Init(player, _soundService);
            _nitroAction.Init(player, _soundService);
            _reloadAction.Init(player, _updateService, _soundService);
        }
    }
}