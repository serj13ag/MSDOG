using System;
using System.Collections;
using UnityEngine;

namespace Core.Controllers
{
    public interface ICoroutineController
    {
        Coroutine StartCoroutine(IEnumerator coroutine, Action onComplete);
    }
}