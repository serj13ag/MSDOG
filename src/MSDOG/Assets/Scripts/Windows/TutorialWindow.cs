using System;
using Core.Models.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Windows
{
    public class TutorialWindow : MonoBehaviour, IWindow
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Image _hintImage;
        [SerializeField] private TMP_Text _hintText;

        public GameObject GameObject => gameObject;

        public event EventHandler<EventArgs> OnCloseRequested;

        public void Init(TutorialEventData tutorialEventData)
        {
            _hintImage.sprite = tutorialEventData.Image;
            _hintText.text = tutorialEventData.HintText;
        }

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