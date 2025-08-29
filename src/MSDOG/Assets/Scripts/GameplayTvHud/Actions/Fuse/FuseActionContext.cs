using Gameplay.Services;
using GameplayTvHud.Mediators;
using UnityEngine;

namespace GameplayTvHud.Actions.Fuse
{
    public class FuseActionContext
    {
        public FuseAction FuseAction { get; }
        public IActionMediator ActionMediator { get; }
        public IInputService InputService { get; }
        public ActionBar ActionBar { get; }
        public Camera HUDCamera { get; }
        public GameObject HandleObject { get; }

        public FuseActionContext(FuseAction fuseAction, IActionMediator actionMediator, IInputService inputService,
            ActionBar actionBar, Camera hudCamera, GameObject handleObject)
        {
            FuseAction = fuseAction;
            ActionMediator = actionMediator;
            InputService = inputService;
            ActionBar = actionBar;
            HUDCamera = hudCamera;
            HandleObject = handleObject;
        }
    }
}