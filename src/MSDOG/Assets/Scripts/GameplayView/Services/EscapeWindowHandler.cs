using System;
using Core.Controllers;
using Gameplay.Services;
using UI.Windows;

namespace GameplayView.Services
{
    public class EscapeWindowHandler : IEscapeWindowHandler, IDisposable
    {
        private readonly IInputService _inputService;
        private readonly IWindowController _windowController;

        public EscapeWindowHandler(IInputService inputService, IWindowController windowController)
        {
            _inputService = inputService;
            _windowController = windowController;
        }

        public void Init()
        {
            _inputService.OnMenuActionPerformed += OnMenuActionPerformed;
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

        public void Dispose()
        {
            _inputService.OnMenuActionPerformed -= OnMenuActionPerformed;
        }
    }
}