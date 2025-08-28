using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Controllers
{
    public class LoadingCurtainController : BasePersistentController, ILoadingCurtainController
    {
        private const float SolidAlpha = 1;
        private const float ClearAlpha = 0;

        [SerializeField] private MaskableGraphic _curtainMaskableGraphic;
        [SerializeField] private float _delay = 1f;
        [SerializeField] private float _timeToFade = 1f;

        private Action _onFadeEndedCallback;

        public void FadeOnInstantly()
        {
            _curtainMaskableGraphic.CrossFadeAlpha(SolidAlpha, 0f, true);
        }

        public void FadeOffWithDelay(Action onFadeEndedCallback = null)
        {
            _onFadeEndedCallback = onFadeEndedCallback;
            StartCoroutine(FadeRoutine(ClearAlpha));
        }

        private IEnumerator FadeRoutine(float alpha)
        {
            yield return new WaitForSeconds(_delay);

            _curtainMaskableGraphic.CrossFadeAlpha(alpha, _timeToFade, true);

            // wait until fade ended
            yield return new WaitForSeconds(_timeToFade);
            _onFadeEndedCallback?.Invoke();
            _onFadeEndedCallback = null;
        }
    }
}