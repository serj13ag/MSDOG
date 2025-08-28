using UnityEngine;
using UnityEngine.UI;

namespace Windows
{
    public abstract class BaseCloseableWindow : BaseWindow
    {
        [SerializeField] private Button _closeButton;

        protected override void OnEnable()
        {
            _closeButton.onClick.AddListener(Close);
        }

        protected override void OnDisable()
        {
            _closeButton.onClick.RemoveListener(Close);
        }
    }
}