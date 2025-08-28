using System;
using System.Collections.Generic;
using Windows;
using Core.Factories;
using UnityEngine;
using VContainer;

namespace Core.Controllers
{
    public class WindowController : BasePersistentController, IWindowController
    {
        [SerializeField] private Canvas _uiCanvasRoot;

        private IGlobalWindowFactory _globalWindowFactory;

        private readonly Stack<IWindow> _activeWindows = new Stack<IWindow>();

        public Transform UiCanvasRootTransform => _uiCanvasRoot.transform;
        public bool HasActiveWindows => _activeWindows.Count > 0;

        public event EventHandler<EventArgs> OnWindowShowed;
        public event EventHandler<EventArgs> OnWindowClosed;

        [Inject]
        public void Construct(IGlobalWindowFactory globalWindowFactory)
        {
            _globalWindowFactory = globalWindowFactory;
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

        public void ShowWindow(IWindow window)
        {
            ShowWindowInner(window);
        }

        public void ShowOptionsWindow()
        {
            var optionsWindow = _globalWindowFactory.CreateOptionsWindow(_uiCanvasRoot.transform);
            ShowWindowInner(optionsWindow);
        }

        public void ShowCreditsWindow()
        {
            var creditsWindow = _globalWindowFactory.CreateCreditsWindow(_uiCanvasRoot.transform);
            ShowWindowInner(creditsWindow);
        }

        public void CloseAllWindows()
        {
            while (_activeWindows.Count > 0)
            {
                CloseActiveWindow();
            }
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

            OnWindowClosed?.Invoke(this, EventArgs.Empty);
        }

        private void ShowWindowInner(IWindow window)
        {
            _activeWindows.Push(window);
            window.OnCloseRequested += OnWindowCloseRequested;

            OnWindowShowed?.Invoke(this, EventArgs.Empty);
        }

        private void OnWindowCloseRequested(object sender, EventArgs e)
        {
            CloseActiveWindow();
        }
    }
}