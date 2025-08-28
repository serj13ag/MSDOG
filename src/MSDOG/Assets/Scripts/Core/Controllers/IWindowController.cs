using System;
using Windows;
using UnityEngine;

namespace Core.Controllers
{
    public interface IWindowController
    {
        Transform UiCanvasRootTransform { get; }
        bool HasActiveWindows { get; }

        event EventHandler<EventArgs> OnWindowShowed;
        event EventHandler<EventArgs> OnWindowClosed;

        bool WindowIsActive<T>() where T : IWindow;

        void ShowWindow(IWindow window);

        void ShowOptionsWindow();
        void ShowCreditsWindow();

        void CloseAllWindows();
        void CloseActiveWindow();
    }
}