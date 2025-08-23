using System;
using Windows;
using Core.Controllers;

namespace Gameplay.Services
{
    public class GameplayWindowsHandler : IGameplayWindowsHandler, IDisposable
    {
        private readonly IInputService _inputService;
        private readonly IWindowController _windowController;
        private readonly IUpdateController _updateController;

        public GameplayWindowsHandler(IInputService inputService, IWindowController windowController,
            IUpdateController updateController)
        {
            _inputService = inputService;
            _windowController = windowController;
            _updateController = updateController;
        }

        public void Init()
        {
            _inputService.OnMenuActionPerformed += OnMenuActionPerformed;
            _windowController.OnWindowShowed += OnWindowShowed;
            _windowController.OnWindowClosed += OnWindowClosed;
        }

        private void OnMenuActionPerformed(object sender, EventArgs e)
        {
            if (_windowController.WindowIsActive<EscapeWindow>())
            {
                _windowController.CloseActiveWindow();
            }
            else
            {
                _windowController.ShowEscapeWindow();
            }
        }

        private void OnWindowShowed(object sender, EventArgs e)
        {
            _inputService.LockInput();
            _updateController.Pause();
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            if (!_windowController.HasActiveWindows)
            {
                _inputService.UnlockInput();
                _updateController.Unpause();
            }
        }

        public void Dispose()
        {
            _inputService.OnMenuActionPerformed -= OnMenuActionPerformed;
            _windowController.OnWindowShowed -= OnWindowShowed;
            _windowController.OnWindowClosed -= OnWindowClosed;
        }
    }
}