using UnityEngine;
using UnityEngine.UI;

namespace Windows
{
    public class CreditsWindow : BaseWindow
    {
        [SerializeField] private Button _closeButton;

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(Close);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(Close);
        }
    }
}