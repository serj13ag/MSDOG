using Core.Controllers;
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

        private UpdateController _updateController;
        private SoundController _soundController;
        private TutorialService _tutorialService;

        [Inject]
        public void Construct(UpdateController updateController, SoundController soundController, TutorialService tutorialService)
        {
            _tutorialService = tutorialService;
            _soundController = soundController;
            _updateController = updateController;
        }

        public void Init(Player player)
        {
            _fuseAction.Init(player, _soundController, _tutorialService);
            _nitroAction.Init(player, _soundController);
            _reloadAction.Init(player, _updateController, _soundController, _tutorialService);
        }
    }
}