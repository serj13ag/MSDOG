using Windows;
using UnityEngine;

namespace Core.Factories
{
    public interface IGlobalWindowFactory
    {
        OptionsWindow CreateOptionsWindow(Transform canvasTransform);
        CreditsWindow CreateCreditsWindow(Transform canvasTransform);
    }
}