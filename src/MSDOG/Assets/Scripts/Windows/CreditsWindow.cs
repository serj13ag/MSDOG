using System;
using UnityEngine;
using UnityEngine.UI;

namespace Windows
{
    public class CreditsWindow : MonoBehaviour, IWindow
    {
        [SerializeField] private Button _closeButton;

        public GameObject GameObject => gameObject;

        public event EventHandler<EventArgs> OnCloseRequested;

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(Close);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(Close);
        }

        private void Close()
        {
            OnCloseRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}