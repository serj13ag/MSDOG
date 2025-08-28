using System;
using Windows;
using Core.Controllers;

namespace Gameplay.Services
{
    public class GameplayWindowsHandler : IGameplayWindowsHandler, IDisposable
    {
        private readonly IInputService _inputService;
        private readonly IWindowController _windowController;
        private readonly IGameSpeedService _gameSpeedService;
        private readonly IGameplayWindowService _gameplayWindowService;

        public GameplayWindowsHandler(IInputService inputService, IWindowController windowController,
            IGameSpeedService gameSpeedService, IGameplayWindowService gameplayWindowService)
        {
            _gameplayWindowService = gameplayWindowService;
            _inputService = inputService;
            _windowController = windowController;
            _gameSpeedService = gameSpeedService;
        }

        public void StartTrackingEvents()
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
                _gameplayWindowService.ShowEscapeWindow();
            }
        }

        private void OnWindowShowed(object sender, EventArgs e)
        {
            _inputService.LockInput();
            _gameSpeedService.Pause();
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            if (!_windowController.HasActiveWindows)
            {
                _inputService.UnlockInput();
                _gameSpeedService.Unpause();
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