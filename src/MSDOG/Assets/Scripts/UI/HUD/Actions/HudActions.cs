using Core.Services;
using Gameplay;
using Gameplay.Services;
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
        private TutorialService _tutorialService;

        [Inject]
        public void Construct(UpdateService updateService, SoundService soundService, TutorialService tutorialService)
        {
            _tutorialService = tutorialService;
            _soundService = soundService;
            _updateService = updateService;
        }

        public void Init(Player player)
        {
            _fuseAction.Init(player, _soundService, _tutorialService);
            _nitroAction.Init(player, _soundService);
            _reloadAction.Init(player, _updateService, _soundService, _tutorialService);
        }
    }
}