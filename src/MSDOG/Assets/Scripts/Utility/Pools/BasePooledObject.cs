using System;
using UnityEngine;

namespace Utility.Pools
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

            Cleanup();
        }

        protected void Release()
        {
            _releaseCallback?.Invoke();
        }

        protected virtual void Cleanup()
        {
        }

        private void OnDestroy()
        {
            Cleanup();
        }
    }
}