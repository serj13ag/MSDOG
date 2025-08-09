using System;

namespace Core.Controllers
{
    public interface ILoadingCurtainController
    {
        void FadeOnInstantly();
        void FadeOffWithDelay(Action onFadeEndedCallback = null);
    }
}