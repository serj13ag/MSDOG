using Windows;
using UnityEngine;

namespace Core.Factories
{
    public interface IGlobalWindowFactory
    {
        IWindow CreateOptionsWindow(Transform canvasTransform);
        IWindow CreateCreditsWindow(Transform canvasTransform);
    }
}