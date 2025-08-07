using System;
using UnityEngine;

namespace Utility
{
    public abstract class BasePooledObject : MonoBehaviour
    {
        private Action _releaseCallback;

        public void SetReleaseCallback(Action releaseCallback)
        {
            _releaseCallback = releaseCallback;
        }

        public virtual void OnGet()
        {
            gameObject.SetActive(true);
        }

        public virtual void OnRelease()
        {
            gameObject.SetActive(false);
        }

        protected void Release()
        {
            _releaseCallback?.Invoke();
        }
    }
}