using Gameplay.Services;
using UnityEngine;

namespace GameplayTvHud.Actions.Reload
{
    public class ReloadActionContext
    {
        public ReloadAction ReloadAction { get; }
        public IInputService InputService { get; }
        public Camera HUDCamera { get; }
        public GameObject HandleObject { get; }

        public ReloadActionContext(ReloadAction reloadAction, IInputService inputService, Camera hudCamera,
            GameObject handleObject)
        {
            ReloadAction = reloadAction;
            HUDCamera = hudCamera;
            HandleObject = handleObject;
            InputService = inputService;
        }
    }
}