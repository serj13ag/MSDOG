using System;
using System.Collections.Generic;
using Services.Gameplay;
using UI.Windows;
using UnityEngine;
using VContainer;

namespace Services
{
    public class WindowService : PersistentMonoService
    {
        [SerializeField] private Canvas _canvas;

        private GlobalWindowFactory _globalWindowFactory;
        private GameplayWindowFactory _gameplayWindowFactory;

        private readonly Stack<IWindow> _activeWindows = new Stack<IWindow>();

        [Inject]
        public void Construct(GlobalWindowFactory globalWindowFactory)
        {
            _globalWindowFactory = globalWindowFactory;
        }

        public void RegisterGameplayWindowFactory(GameplayWindowFactory gameplayWindowFactory)
        {
            _gameplayWindowFactory = gameplayWindowFactory;
        }

        public void RemoveGameplayWindowFactory()
        {
            _gameplayWindowFactory = null;
        }

        public bool WindowIsActive<T>() where T : IWindow
        {
            foreach (var activeWindow in _activeWindows)
            {
                if (activeWindow is T)
                {
                    return true;
                }
            }

            return false;
        }

        public void ShowOptions()
        {
            var optionsWindow = _globalWindowFactory.CreateOptionsWindow(_canvas.transform);
            ShowWindow(optionsWindow);
        }

        public void ShowEscapeWindow()
        {
            var escapeWindow = _gameplayWindowFactory.CreateEscapeWindow(_canvas.transform);
            ShowWindow(escapeWindow);
        }

        public void ShowWinWindow()
        {
            var winWindow = _gameplayWindowFactory.CreateWinWindow(_canvas.transform);
            ShowWindow(winWindow);
        }

        public void ShowLoseWindow()
        {
            var loseWindow = _gameplayWindowFactory.CreateLoseWindow(_canvas.transform);
            ShowWindow(loseWindow);
        }

        public void CloseActiveWindow()
        {
            if (_activeWindows.Count == 0)
            {
                Debug.LogError("Has no active windows!");
                return;
            }

            var windowToClose = _activeWindows.Pop();
            windowToClose.OnCloseRequested -= OnWindowCloseRequested;
            Destroy(windowToClose.GameObject);
        }

        private void ShowWindow(IWindow window)
        {
            _activeWindows.Push(window);
            window.OnCloseRequested += OnWindowCloseRequested;
        }

        private void OnWindowCloseRequested(object sender, EventArgs e)
        {
            CloseActiveWindow();
        }
    }
}