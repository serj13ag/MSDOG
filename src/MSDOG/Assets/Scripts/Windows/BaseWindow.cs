using System;
using UnityEngine;

namespace Windows
{
    public abstract class BaseWindow : MonoBehaviour, IWindow
    {
        public GameObject GameObject => gameObject;

        public event EventHandler<EventArgs> OnCloseRequested;

        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
        }

        protected void Close()
        {
            OnCloseRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}