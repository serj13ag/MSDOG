using System;
using Data;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI.Windows
{
    public class TutorialWindow : MonoBehaviour, IWindow
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Image _hintImage;
        [SerializeField] private TMP_Text _hintText;

        private UpdateService _updateService;

        public GameObject GameObject => gameObject;

        public event EventHandler<EventArgs> OnCloseRequested;

        [Inject]
        public void Construct(UpdateService updateService)
        {
            _updateService = updateService;
        }

        public void Init(TutorialEventData tutorialEventData)
        {
            _hintImage.sprite = tutorialEventData.Image;
            _hintText.text = tutorialEventData.HintText;
        }

        private void OnEnable()
        {
            _updateService.Pause(true);

            _closeButton.onClick.AddListener(Close);
        }

        private void OnDisable()
        {
            _updateService.Unpause(true);

            _closeButton.onClick.RemoveListener(Close);
        }

        private void Close()
        {
            OnCloseRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}