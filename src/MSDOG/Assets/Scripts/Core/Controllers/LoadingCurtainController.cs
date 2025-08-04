using System;
using System.Collections;
using Constants;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Controllers
{
    public class LoadingCurtainController : BasePersistentController
    {
        [SerializeField] private MaskableGraphic _curtainMaskableGraphic;

        private Action _onFadeEndedCallback;

        public void FadeOnInstantly()
        {
            _curtainMaskableGraphic.CrossFadeAlpha(Settings.ScreenFader.SolidAlpha, 0f, true);
        }

        public void FadeOffWithDelay(Action onFadeEndedCallback = null)
        {
            _onFadeEndedCallback = onFadeEndedCallback;
            StartCoroutine(FadeRoutine(Settings.ScreenFader.ClearAlpha));
        }

        private IEnumerator FadeRoutine(float alpha)
        {
            yield return new WaitForSeconds(Settings.ScreenFader.Delay);

            _curtainMaskableGraphic.CrossFadeAlpha(alpha, Settings.ScreenFader.TimeToFade, true);

            // wait until fade ended
            yield return new WaitForSeconds(Settings.ScreenFader.TimeToFade);
            _onFadeEndedCallback?.Invoke();
            _onFadeEndedCallback = null;
        }
    }
}