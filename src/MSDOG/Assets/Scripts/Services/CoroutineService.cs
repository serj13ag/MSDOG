using System;
using System.Collections;
using UnityEngine;

namespace Services
{
    public class CoroutineService : BaseMonoService
    {
        public Coroutine StartCoroutine(IEnumerator coroutine, Action onComplete)
        {
            return StartCoroutine(Run(coroutine, onComplete));
        }

        private IEnumerator Run(IEnumerator coroutine, Action onComplete)
        {
            yield return StartCoroutine(coroutine);
            onComplete?.Invoke();
        }
    }
}